#include "settingswindow.h"
#include "ui_settingswindow.h"
#include "../../interop/appmanager.h"
#include "../../utils/binarykv.h"
#include "../../threading/threadcontroller.h"
#include <QMessageBox>
#include <opensteamworks/IClientCompat.h>

SettingsWindow::SettingsWindow(QWidget *parent, App *app) :
    QDialog(parent),
    ui(new Ui::SettingsWindow)
{
    ui->setupUi(this);
    this->app = app;
    setWindowTitle(QString::fromStdString(app->name).append(" settings"));
    bool compatEnabled = app->GetCompatEnabled();
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

void SettingsWindow::PopulateBetas() {
    ui->betasDropdown->blockSignals(true);

    ui->betasDropdown->clear();
    dropdownBetas.clear();

    ui->betasDropdown->addItem("NONE - No beta selected");

    std::vector<Beta> betas = app->GetAllBetas();

    if (betas.size() == 0) {
        ui->betasDropdown->setCurrentIndex(0);
        return;
    }

    std::string currentBeta = app->GetCurrentBeta();
    DEBUG_MSG << "[SettingsWindow] beta is " << currentBeta << std::endl;

    for (auto &&beta : betas)
    {
        std::string flags = "";

        //TODO: give the user an option to hide betas they don't have access to
        if (beta.hasAccess && beta.pwdrequired) {
            flags.append("Private");
        } else if (beta.pwdrequired) {
            flags.append("Password Required");
        }

        if (!flags.empty()) {
            flags = "[" + flags + "]";
        }
        ui->betasDropdown->addItem(QString("%1 - %2 %3").arg(QString::fromStdString(beta.name), QString::fromStdString(beta.description), QString::fromStdString(flags)));
        dropdownBetas.insert({ui->betasDropdown->count()-1, beta});

        if (beta.name == std::string(currentBeta)) {
            ui->betasDropdown->setCurrentIndex(ui->betasDropdown->count()-1);
        }
    }
    
    ui->betasDropdown->blockSignals(false);
}

void SettingsWindow::ReadLaunchOptions() {
    ui->launchOptionsField->blockSignals(true);
    ui->launchOptionsField->setText(QString::fromStdString(std::string(app->GetLaunchCommandLine())));
    ui->launchOptionsField->blockSignals(false);
}

void SettingsWindow::PopulateCompatTools()
{
    ui->compatToolBox->blockSignals(true);

    ui->compatToolBox->clear();

    int selectedIndex = -1;
    for (auto &&i : app->GetAllowedCompatTools())
    {
        std::string humanName = i;
        {
            char *humanNameC = Global_SteamClientMgr->ClientCompat->GetCompatToolDisplayName(i.c_str());
            if (humanNameC != nullptr) {
                humanName = std::string(humanNameC);
            }
        }
        ui->compatToolBox->addItem(QString::fromStdString(humanName), QVariant(QString::fromStdString(i)));
        if (selectedIndex == -1 && app->GetCompatEnabled()) {
            if (i == app->GetCurrentCompatTool()) {
                selectedIndex = ui->compatToolBox->count() - 1;
            }
        }
    }
   
    if (selectedIndex != -1) {
        ui->compatToolBox->setCurrentIndex(selectedIndex);
    }

    ui->compatToolBox->blockSignals(false);
}

void SettingsWindow::on_enableProtonBox_stateChanged(int arg1)
{
    ui->compatToolBox->setVisible((bool)arg1);
    if ((bool)arg1) {
        PopulateCompatTools();
    } else {
        app->ClearCompatTool();
    }
}



void SettingsWindow::on_compatToolBox_currentIndexChanged(int index)
{
    QString compatToolName = ui->compatToolBox->itemData(index).toString();

    app->SetCompatTool(compatToolName.toStdString());
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

    if (resp.eResult == k_EResultFailure) {
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
    DEBUG_MSG << "[SettingsWindow] Requested index " << index << std::endl;
    // User selected "No beta"
    if (index == 0) {
        DEBUG_MSG << "[SettingsWindow] index was 0" << std::endl;
        Global_SteamClientMgr->ClientAppManager->SetActiveBeta(app->appid, "public");
        return;
    }

    Beta beta = dropdownBetas.at(index);

    // This calls ResolveDepotDependencies (internally) and also queues the app for immediate update (if installed)
    DEBUG_MSG << "[SettingsWindow] Setting beta to " << beta.name.c_str() << std::endl;
    app->SetCurrentBeta(beta.name);
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
