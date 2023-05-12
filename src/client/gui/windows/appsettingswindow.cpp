#include "appsettingswindow.h"
#include "ui_appsettingswindow.h"
#include "../../ext/steamclient.h"
#include <QTabBar>
#include <QFileDialog>
#include <opensteamworks/IClientAppManager.h>
#include <opensteamworks/IClientCompat.h>

AppSettingsWindow::AppSettingsWindow(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AppSettingsWindow)
{
    ui->setupUi(this);

    LoadLibraryFolders();
    LoadCheckboxValues();
}

void AppSettingsWindow::LoadLibraryFolders() 
{
    Global_SteamClientMgr->ClientAppManager->RefreshLibraryFolders();
    ui->libraryFolders->clear();
    char *path = new char[4096];
    char *label = new char[256];

    for (size_t i = 0; i < Global_SteamClientMgr->ClientAppManager->GetNumLibraryFolders(); i++)
    {
        auto item = new QListWidgetItem();

        Global_SteamClientMgr->ClientAppManager->GetLibraryFolderPath(i, path, 4096);
        Global_SteamClientMgr->ClientAppManager->GetLibraryFolderLabel(i, label, 256);

        std::cout << std::to_string(i).append(": ").append(path).append("(").append(label).append(")") << std::endl;

        item->setText(QString::fromStdString(std::to_string(i).append(": ").append(path).append(" (").append(label).append(")")));
        item->setData(Qt::UserRole, QVariant((int)i));

        ui->libraryFolders->addItem(item);

    }

    delete[] path;
    delete[] label;
    
    ui->libraryFolders->setCurrentRow(0);
}

void AppSettingsWindow::LoadCheckboxValues() {
    ui->enableCompatToolsCheck->setChecked(Global_SteamClientMgr->ClientCompat->BIsCompatLayerEnabled());
    ui->allowDownloadsWhilePlayingCheck->setChecked(Global_SteamClientMgr->ClientAppManager->BAllowDownloadsWhileAnyAppRunning());
}

AppSettingsWindow::~AppSettingsWindow()
{
    delete ui;
}

void AppSettingsWindow::on_libraryFolders_currentItemChanged(QListWidgetItem *current, QListWidgetItem *previous)
{
    if (ui->libraryFolders->currentItem() == nullptr) {
        return;
    }

    //TODO: show installed games and the library folder size 
    
    //LibraryFolder_t folder = current->data(Qt::UserRole).toInt();

}

void AppSettingsWindow::on_addFolderButton_clicked()
{
    QString path = QFileDialog::getExistingDirectory(this, ("Select library folder"), QDir::currentPath());
    if (path.isEmpty()) {
        return;
    }

    std::string pathStr = path.toStdString();
    Global_SteamClientMgr->ClientAppManager->AddLibraryFolder(pathStr.c_str());
    LoadLibraryFolders();
}

void AppSettingsWindow::on_deleteFolderButton_clicked()
{   
    if (ui->libraryFolders->currentItem() == nullptr) {
        return;
    }
    LibraryFolder_t folder = ui->libraryFolders->currentItem()->data(Qt::UserRole).toInt();
    // Note: no idea what those two bools do, but the official client uses false for both
    Global_SteamClientMgr->ClientAppManager->RemoveLibraryFolder(folder, false, false);
    LoadLibraryFolders();
}


void AppSettingsWindow::on_enableCompatToolsCheck_stateChanged(int arg1)
{
    Global_SteamClientMgr->ClientCompat->EnableCompat((bool)arg1);
}


void AppSettingsWindow::on_allowDownloadsWhilePlayingCheck_stateChanged(int arg1)
{
    Global_SteamClientMgr->ClientAppManager->SetAllowDownloadsWhileAnyAppRunning((bool)arg1);
}

