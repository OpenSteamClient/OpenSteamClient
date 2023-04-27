#include "settingswindow.h"
#include "ui_settingswindow.h"
#include "../../interop/appmanager.h"
#include "../../utils/binarykv.h"
#include "../../threading/threadcontroller.h"
#include <QMessageBox>

SettingsWindow::SettingsWindow(QWidget *parent, App *app) :
    QDialog(parent),
    ui(new Ui::SettingsWindow)
{
    ui->setupUi(this);
    this->app = app;
    setWindowTitle(QString::fromStdString(app->name).append(" settings"));
    bool compatEnabled = Global_SteamClientMgr->ClientCompat->BIsCompatibilityToolEnabled(app->appid);
    ui->enableProtonBox->setChecked(compatEnabled);
    ui->compatToolBox->setVisible(compatEnabled);
    if (compatEnabled) {
        PopulateCompatTools();
    }
    PopulateBetas();
    ReadLaunchOptions();
}

SettingsWindow::~SettingsWindow()
{
    delete ui;
}

std::map<int, Beta> betas;
void SettingsWindow::PopulateBetas() {
    ui->betasDropdown->clear();
    betas.clear();

    ui->betasDropdown->addItem("NONE - No beta selected");

    size_t bufLen = 1000000;
    std::vector<uint8_t> buf(bufLen);

    //int returnedLength = Global_SteamClientMgr->ClientAppManager->GetAvailableBetas(app->appid, &success, betas, sizeof(betas), 0);
    int returnedLength = Global_SteamClientMgr->ClientApps->GetAppDataSection(app->appid, k_EAppInfoSectionDepots, buf.data(), bufLen, false);

    if (returnedLength <= 0) {
        ui->betasDropdown->setCurrentIndex(0);
        return;
    }

    BinaryKV *bkv = new BinaryKV(buf);
    DEBUG_MSG << bkv->outputJSON << std::endl;

    char currentBeta[256];
    Global_SteamClientMgr->ClientAppManager->GetActiveBeta(app->appid, currentBeta, sizeof(currentBeta));
    DEBUG_MSG << "beta is " << currentBeta << std::endl;
    for (auto &&i : bkv->outputJSON["depots"]["branches"].items())
    {
        Beta beta;

        beta.name = i.key();
        if (beta.name == "public") {
            continue;
        }

        beta.buildid = -1;
        beta.pwdrequired = false;
        beta.timeupdated = -1;

        std::string flags = "";

        if (i.value().contains("description")) {
            beta.description = i.value()["description"];
        } else {
            beta.description = "Missing Description";
        }

        if (i.value().contains("pwdrequired")) {
            beta.pwdrequired = (bool)(int)i.value()["pwdrequired"];

            //TODO: give the user an option to hide betas they don't have access to
            if (Global_SteamClientMgr->ClientAppManager->BHasCachedBetaPassword(app->appid, beta.name.c_str())) {
                flags.append("Private");
            }
            else
            {
                flags.append("Password Required");
            }
        }

        if (i.value().contains("timeupdated")) {
            beta.timeupdated = i.value()["timeupdated"];
        }

        if (!flags.empty()) {
            flags = "[" + flags + "]";
        }

        ui->betasDropdown->addItem(QString("%1 - %2 %3").arg(QString::fromStdString(beta.name), QString::fromStdString(beta.description), QString::fromStdString(flags)));
        betas.insert({ui->betasDropdown->count()-1, beta});
        if (beta.name == std::string(currentBeta)) {
            ui->betasDropdown->setCurrentIndex(ui->betasDropdown->count()-1);
        }
    }
}

void SettingsWindow::ReadLaunchOptions() {
    std::string path = std::string("Software/Valve/Steam/Apps/").append(std::to_string(app->appid)).append("/LaunchOptions");

    const char *launchOpts = Global_SteamClientMgr->ClientConfigStore->GetString(k_EConfigStoreUserLocal, path.c_str(), "");
    //Global_SteamClientMgr->ClientAppManager->GetAppConfigValue(app->appid, "launchoptions", launchOpts, sizeof(launchOpts));
    ui->launchOptionsField->setText(QString::fromStdString(std::string(launchOpts)));
}

void SettingsWindow::PopulateCompatTools()
{
    ui->compatToolBox->clear();

    CUtlVector<CUtlString> *vec = new CUtlVector<CUtlString>(1, 1000000);

    Global_SteamClientMgr->ClientCompat->GetAvailableCompatToolsForApp(vec, app->appid);

    int selectedIndex = -1;
    for (size_t i = 0; i < vec->Count(); i++)
    {
        if (vec->Element(i).str == nullptr || vec->Element(i).str == NULL) {
            DEBUG_MSG << "Name is nullptr" << std::endl;
        }
        else
        {
            char *name = reinterpret_cast<char*>(vec->Element(i).str);
            char *humanName = Global_SteamClientMgr->ClientCompat->GetCompatToolDisplayName(name);
            ui->compatToolBox->addItem(QString::fromStdString(std::string(humanName)), QVariant(QString::fromStdString(std::string(name))));
            if (selectedIndex == -1 && Global_SteamClientMgr->ClientCompat->BIsCompatibilityToolEnabled(app->appid)) {
                if (std::string(name) == std::string(Global_SteamClientMgr->ClientCompat->GetCompatToolName(app->appid))) {
                    selectedIndex = ui->compatToolBox->count()-1;
                }
            }
        }
    }
    if (selectedIndex != -1) {
        ui->compatToolBox->setCurrentIndex(selectedIndex);
    }
}

void SettingsWindow::on_enableProtonBox_stateChanged(int arg1)
{
    ui->compatToolBox->setVisible((bool)arg1);
    if ((bool)arg1) {
        PopulateCompatTools();
    } else {
        Global_SteamClientMgr->ClientCompat->SpecifyCompatTool(app->appid, "", "", 0);
    }
}



void SettingsWindow::on_compatToolBox_currentIndexChanged(int index)
{
    QString compatToolName = ui->compatToolBox->itemData(index).toString();
    QString compatToolHumanName = ui->compatToolBox->itemText(index);

    Global_SteamClientMgr->ClientCompat->SpecifyCompatTool(app->appid, compatToolName.toStdString().c_str(), compatToolHumanName.toStdString().c_str(), 0);
}

void SettingsWindow::on_testBetaKeyButton_clicked()
{
    if (ui->betaKeyBox->text().isEmpty()) {
        return;
    }

    std::string password = ui->betaKeyBox->text().toStdString();
    Global_SteamClientMgr->ClientAppManager->CheckBetaPassword(app->appid, password.c_str());

    connect(Global_ThreadController->callbackThread, &CallbackThread::CheckAppBetaPasswordResponse, this, &SettingsWindow::betaPasswordResponseReceived, Qt::ConnectionType::SingleShotConnection);
}

void SettingsWindow::betaPasswordResponseReceived(CheckAppBetaPasswordResponse_t resp) {
    QMessageBox msgBox;

    if (resp.eResult == k_EResultFail) {
        msgBox.setText("Beta password invalid. ");
    } else if (resp.eResult == k_EResultOK) {
        PopulateBetas();
        msgBox.setText("Beta password valid, new betas available.");
    } else {
        msgBox.setText(QString("Unexpected error code ").arg(resp.eResult));
    }
    msgBox.exec();
}

void SettingsWindow::on_betasDropdown_activated(int index)
{
    DEBUG_MSG << "Requested index " << index << std::endl;
    // User selected "No beta"
    if (index == 0) {
        DEBUG_MSG << "index was 0" << std::endl;
        Global_SteamClientMgr->ClientAppManager->SetAppConfigValue(app->appid, "betakey", "public");
        return;
    }

    Beta beta = betas.at(index);

    // This calls ResolveDepotDependencies (internally) and also queues the app for immediate update (if installed)
    DEBUG_MSG << "Setting beta to " << beta.name.c_str() << std::endl;
    const char *betaNameReal = beta.name.c_str();

    //Global_SteamClientMgr->ClientAppManager->SetAppConfigValue(app->appid, "BetaKey", betaNameReal);
    //Global_SteamClientMgr->ClientAppManager->SetAppConfigValue(app->appid, "branch", betaNameReal);
    Global_SteamClientMgr->ClientAppManager->SetAppConfigValue(app->appid, "betakey", betaNameReal);


    char currentBeta[256];
    Global_SteamClientMgr->ClientAppManager->GetActiveBeta(app->appid, currentBeta, sizeof(currentBeta));
    DEBUG_MSG << "beta is " << currentBeta << std::endl;
}

void SettingsWindow::on_uninstallBtn_clicked()
{
    QMessageBox *msgBox = new QMessageBox(this);
    msgBox->setIcon(QMessageBox::Icon::Question);
    msgBox->setText(QString("Are you sure you wish to uninstall %1?").arg(QString::fromStdString(app->name)));
    msgBox->setStandardButtons(QMessageBox::StandardButtons(QMessageBox::StandardButton::No | QMessageBox::StandardButton::Yes));
    switch (msgBox->exec()) {
        case QMessageBox::Yes:
            app->Uninstall();
            this->close();
            this->deleteLater();
            break;
        case QMessageBox::No:
        default:
        break;

    }
}

void SettingsWindow::on_launchOptionsField_editingFinished()
{
    std::string newOpt = ui->launchOptionsField->text().toStdString();
    this->app->SetLaunchCommandLine(newOpt);
}
