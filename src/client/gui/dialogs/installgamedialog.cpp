#include "installgamedialog.h"
#include "ui_installgamedialog.h"
#include "../../ext/steamclient.h"
#include <QAbstractButton>
#include <QMessageBox>

#include <filesystem>
namespace fs = std::filesystem;

InstallGameDialog::InstallGameDialog(QWidget *parent, App *app) : QWizard(parent),
                                                                 ui(new Ui::InstallGameDialog)
{
    ui->setupUi(this);
    this->app = app;

    // This is a hack we do to disable the next button until a library folder is selected
    ui->disableNextButtonHack->setVisible(false);

    this->setWindowTitle(this->windowTitle().arg(QString::fromStdString(app->name)));
    PopulateLibraryFolders();

    //TODO: get game size
}

std::map<int, LibraryFolder> libraryFolders;
void InstallGameDialog::PopulateLibraryFolders() {
    libraryFolders.clear();
    ui->installLocationBox->clear();
    ui->installLocationBox->addItem("No library folder selected");

    // TODO: get size of game

    for (LibraryFolder_t i = 0; i < Global_SteamClientMgr->ClientAppManager->GetNumLibraryFolders(); i++)
    {
        LibraryFolder info;
        info.folderIndex = i;

        // Get label info
        char *label = new char[1024];
        Global_SteamClientMgr->ClientAppManager->GetLibraryFolderLabel(i, label, 1024);
        info.label = std::string(label);
        delete[] label;

        // Get path
        char *path = new char[1024];
        Global_SteamClientMgr->ClientAppManager->GetLibraryFolderPath(i, path, 1024);
        info.path = std::string(path);
        delete[] path;

        // Get free space info
        fs::space_info spaceinfo = fs::space(info.path);
        info.freeSpaceHuman = std::to_string((float)(spaceinfo.available / (float)1024 / (float)1024 / (float)1024)).append(" GB");

        //TODO: See if we can fit the game
        info.canFitGame = true;

        ui->installLocationBox->addItem(QString("%1: %2 (Available: %3)").arg(QString::fromStdString(std::to_string(info.folderIndex)), QString::fromStdString(info.path), QString::fromStdString(info.freeSpaceHuman)));
        libraryFolders.insert({ui->installLocationBox->count()-1, info});
    }
}

InstallGameDialog::~InstallGameDialog()
{
    delete ui;
}

void InstallGameDialog::on_installLocationBox_activated(int index)
{
    if (index == 0) {
        ui->spaceAvailableLabel->setText("Select a library folder to see space information");
        return;
    }
    ui->spaceAvailableLabel->setText(QString("Disk space available: %1").arg(QString::fromStdString(libraryFolders.at(index).freeSpaceHuman)));
}

void InstallGameDialog::on_InstallGameDialog_currentIdChanged(int id)
{
    // We are exiting this dialog, allow it
    if (id == -1) {
        return;
    }

    // If we're switching to the first page, do it no questions asked
    if (id == 0) {
        return; 
    }

    if (ui->installLocationBox->currentIndex() == 0)
    {
        this->setCurrentId(0);
        QMessageBox msgBox;
        msgBox.setText("No library folder selected. Please pick a folder before continuing.");
        msgBox.exec();
        return;
    }

    LibraryFolder folder = libraryFolders.at(ui->installLocationBox->currentIndex());

    if (!folder.canFitGame) {
        this->setCurrentId(0);
        QMessageBox msgBox;
        msgBox.setText("Not enough available space in target drive.");
        msgBox.exec();
        return;
    }

    connect(this->app, &App::AppLaunchOrUpdateError, this, &InstallGameDialog::installFailed);
    app->Install(folder);
}

void InstallGameDialog::installFailed(EAppUpdateError err) {
    if (err == 0)
        return;

    this->setCurrentId(0);
    QMessageBox msgBox;
    msgBox.setText(QString("Failed to install: %1").arg(err));
    msgBox.exec();
    disconnect(qobject_cast<App *>(sender()), &App::AppLaunchOrUpdateError, this, &InstallGameDialog::installFailed);
}