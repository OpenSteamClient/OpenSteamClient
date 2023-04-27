#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "../application.h"
#include "../../messaging/protomsg.hpp"
#include <steammessages_clientserver_appinfo.pb.h>
#include <enums_clientserver.pb.h>
#include "../../interop/appmanager.h"
#include "../dialogs/launchoptionsdialog.h"
#include "../../interop/app.h"
#include <QListWidget>
#include "settingswindow.h"
#include "../dialogs/installgamedialog.h"
#include <QMessageBox>
#include "../widgets/downloadqueueitem.h"
#include "../gamelistview/appdelegate.h"
#include "../gamelistview/appmodel.h"
#include "../gamelistview/treeitem.h"
#include "../../interop/errmsgutils.h"


MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    // Steam | View | Friends | Games | Help
    auto steamMenu = menuBar()->addMenu("Steam");

    auto changeAccountAct = new QAction("Change Account", this);
    auto signOutAct = new QAction("Sign out", this);
    auto goOfflineAction = new QAction("Go offline", this);
    auto settingsAction = new QAction("Settings", this);
    auto quitAction = new QAction("Quit", this);
    auto quitAndRestoreValveSteamAction = new QAction("Quit and restore ValveSteam", this);

    changeAccountAct->setStatusTip("Log out, remember credentials and bring up account switcher");
    signOutAct->setStatusTip("Log out, forget credentials and bring up login screen");

    goOfflineAction->setStatusTip("Enter offline mode");
    settingsAction->setStatusTip("Open the OpenSteam settings window");
    quitAction->setStatusTip("Exit OpenSteam and all running games");
    quitAndRestoreValveSteamAction->setToolTip("Exit OpenSteam and restore the official client");

    connect(quitAction, &QAction::triggered, Application::GetApplication(), &Application::quitApp);
    connect(quitAndRestoreValveSteamAction, &QAction::triggered, Application::GetApplication(), &Application::quitApp);
    connect(changeAccountAct, &QAction::triggered, this, &MainWindow::changeAccount);
    connect(settingsAction, &QAction::triggered, this, &MainWindow::openSettings);

    steamMenu->addAction(changeAccountAct);
    steamMenu->addAction(signOutAct);
    steamMenu->addAction(goOfflineAction);
    steamMenu->addSeparator();
    steamMenu->addAction(settingsAction);
    steamMenu->addSeparator();
    steamMenu->addAction(quitAction);
    steamMenu->addAction(quitAndRestoreValveSteamAction);

    ui->menubar->addMenu(steamMenu);
    
    //TODO: this should be somewhere else
    Global_SteamClientMgr->ClientAppManager->SetDownloadingEnabled(true);

    ui->DEBUGenableProtonChecck->setChecked(Global_SteamClientMgr->ClientCompat->BIsCompatLayerEnabled());
    ui->enableDownloadsBox->setChecked(Global_SteamClientMgr->ClientAppManager->BIsDownloadingEnabled());
    ui->allowDownloadsWhilePlayingBox->setChecked(Global_SteamClientMgr->ClientAppManager->BAllowDownloadsWhileAnyAppRunning());

    LoadApps();

    connect(ui->appsListView->selectionModel(), &QItemSelectionModel::currentChanged, this, &MainWindow::currentItemChanged);
    connect(Global_ThreadController->downloadInfoThread, &DownloadInfoThread::DownloadSpeedUpdate, this, &MainWindow::updateDownloadSpeed);
    connect(Global_ThreadController->downloadInfoThread, &DownloadInfoThread::DownloadingAppChange, this, &MainWindow::currentDownloadingAppChanged);

    // Start and register the thread at this point so we don't miss 
    Global_ThreadController->initThread(Global_ThreadController->downloadInfoThread);
    Global_ThreadController->StartThread(Global_ThreadController->downloadInfoThread);

    // This turns the bottom section downloads QWidget into a clickable one
    this->bottomDownloadSection = new ClickableQWidget(this);
    ui->bottomSection->insertWidget(ui->bottomSection->indexOf(ui->bottomDownloadSection), this->bottomDownloadSection);
    this->bottomDownloadSection->ReplaceExistingQWidget(ui->bottomDownloadSection);
    connect(this->bottomDownloadSection, &ClickableQWidget::clicked, this, &MainWindow::openDownloads);

    // Sets the app list view to be aligned nicely
    ui->appsListView->setIndentation(24);

    // this->scene = new QGraphicsScene();
    // this->view = new QGraphicsView(this->scene, ui->graphicsViewContainer);
    
    // //QNetworkRequest req;
    // //this->manager->get(req);

    // //this->heroItem = new QGraphicsPixmapItem(QPixmap::fromImage(image));
    // //this->scene->addItem(this->heroItem);
    // this->view->show();
    // //QGraphicsPixmapItem item;
    // //ui->mainArtworkGraphicsView->fitInView(item, Qt::AspectRatioMode::KeepAspectRatioByExpanding);
}

void MainWindow::openDownloads() {
    ui->tabWidget->setCurrentIndex(ui->tabWidget->indexOf(ui->downloadsTab));
}

void LogAppUpdateInfo(AppUpdateInfo_s appUpdateInfo) {
      DEBUG_MSG << "AppUpdateInfo_s: { m_timeUpdateStart=" << appUpdateInfo.m_timeUpdateStart
          << ", m_uUnk0=" << appUpdateInfo.m_uUnk0
          << ", m_unBytesToDownload=" << appUpdateInfo.m_unBytesToDownload
          << ", m_unBytesDownloaded=" << appUpdateInfo.m_unBytesDownloaded
          << ", m_unBytesToProcess=" << appUpdateInfo.m_unBytesToProcess
          << ", m_unBytesProcessed=" << appUpdateInfo.m_unBytesProcessed
          << ", m_unBytesToProcess2=" << appUpdateInfo.m_unBytesToProcess2
          << ", m_unBytesProcessed2=" << appUpdateInfo.m_unBytesProcessed2
          << ", m_uUnk3=" << appUpdateInfo.m_uUnk3
          << ", m_uUnk4=" << appUpdateInfo.m_uUnk4
          << ", m_uUnk5=" << appUpdateInfo.m_uUnk5
          << ", m_uUnk6=" << appUpdateInfo.m_uUnk6
          << ", m_uUnk7=" << appUpdateInfo.m_uUnk7
          << ", m_uUnk8=" << appUpdateInfo.m_uUn8
          << ", m_uUnk9=" << appUpdateInfo.m_uUnk9
          << ", m_uUnk10=" << appUpdateInfo.m_uUnk10
          << ", m_uUnk11=" << appUpdateInfo.m_uUnk11
          << ", m_uUnk12=" << appUpdateInfo.m_uUnk12
          << ", m_uUnk13=" << appUpdateInfo.m_uUnk13
          << ", m_uUnk14=" << appUpdateInfo.m_uUnk14
          << ", m_uUnk15=" << appUpdateInfo.m_uUnk15
          << ", m_uUnk16=" << appUpdateInfo.m_uUnk16
          << " }" << std::endl;
}

void MainWindow::updateDownloadSpeed(DownloadSpeedInfo info)
{
    std::string speed = std::to_string((float)(info.downloadSpeed / (float)1024 / (float)1024)).substr(0, 4);
    std::string topSpeed = std::to_string((float)(info.topDownloadSpeed / (float)1024 / (float)1024)).substr(0, 4);
    std::string totalDownloaded = std::to_string((float)(info.totalDownloaded / (float)1024 / (float)1024 / (float)1024)).substr(0, 4);
    QString unit = "MB/s";
    QString totalUnit = "GB";

    ui->topDownloadSpeedLabel->setText(QString("%1 %2").arg(QString::fromStdString(topSpeed), unit));
    ui->downloadSpeedLabel->setText(QString("%1 %2").arg(QString::fromStdString(speed), unit));
    ui->appsToDownloadCounter->display(Global_SteamClientMgr->ClientAppManager->GetNumAppsInDownloadQueue());
    ui->downloadTotal->setText(QString("%1 %2").arg(QString::fromStdString(totalDownloaded), totalUnit));
    //ui->bottomQueuedLabel->setText(QString("Updates: %1 Queued: %2").arg(QString::fromStdString(std::to_string(updatesPerAppId.size())), QString::fromStdString(std::to_string(Global_SteamClientMgr->ClientAppManager->GetNumAppsInDownloadQueue()))));

    UpdateBottomDownloadsBar();
}

void MainWindow::currentDownloadingAppChanged(AppId_t appid) 
{
    DEBUG_MSG << "current downloading app changed" << std::endl;
    if (appid != 0)
    {
        char name[512];
        Global_SteamClientMgr->ClientApps->GetAppData(appid, "common/name", name, sizeof(name));
        ui->bottonDownloadingLabel->setText(QString::fromStdString(std::string(name)));
    }
    else
    {
        ui->bottonDownloadingLabel->setText(QString("No download"));
    }

    UpdateBottomDownloadsBar();
}

void MainWindow::UpdateAppState(AppId_t) {

}

void MainWindow::UpdateDownloadQueue() {
    // for (auto &&i : updatesPerAppId)
    // {   

    //     if (downloadQueueItems.contains(i.first)) {
    //         AppUpdateInfo_s appUpdateInfo;
    //         Global_SteamClientMgr->ClientAppManager->GetUpdateInfo(i.first, &appUpdateInfo);
    //         downloadQueueItems.at(i.first).second->UpdateUpdateInfo(appUpdateInfo);
    //     }
    //      auto item = new QListWidgetItem();
    //         auto widgetitem = new DownloadQueueItem();
    //         widgetitem->SetItemData({
    //             .appid = i.first,
    //             .name = std::string(name),
    //             .initialUpdateInfo = appUpdateInfo
    //         });

    //         downloadQueueItems.insert({i, widgetitem});

    //         item->setSizeHint(widgetitem->sizeHint());

    //         ui->downloadQueueList->addItem(item);
    //         ui->downloadQueueList->setItemWidget(item, widgetitem);
    // }
}

void MainWindow::UpdateBottomDownloadsBar() {
    AppId_t currentAppId = Global_SteamClientMgr->ClientAppManager->GetDownloadingAppID();

    if (currentAppId != 0) {
        AppUpdateInfo_s updateInfo;
        Global_SteamClientMgr->ClientAppManager->GetUpdateInfo(currentAppId, &updateInfo);
        DEBUG_MSG << currentAppId << ": ";
        LogAppUpdateInfo(updateInfo);

        // if (downloadQueueItems.contains(currentAppId)) {
        //     downloadQueueItems.at(currentAppId).second().UpdateUpdateInfo(updateInfo);
        // } else {
        //     std::cout << "Download queue doesn't contain app but we are downloading it?" << std::endl;
        // }

        // Calculate the download progress
        if (updateInfo.m_unBytesToDownload == 0) {
            ui->bottomDownloadingProgress->setValue(0);
            return;
        }

        if (updateInfo.m_unBytesDownloaded == 0) {
            return; 
        }

        double divideResult = (double)updateInfo.m_unBytesDownloaded / (double)updateInfo.m_unBytesToDownload;
        int percentDownloaded = static_cast<int>(divideResult * 1000);
        DEBUG_MSG << "d: " << divideResult << ", p: " << percentDownloaded << std::endl;
        ui->bottomDownloadingProgress->setValue(percentDownloaded);
    }
}

void MainWindow::changeAccount() {
    hide();
    Application::GetApplication()->loginWindow->logoutAndShowWindow();
}

void MainWindow::LoadApps() {
    ui->appsListView->setItemDelegate(new AppDelegate);
    ui->appsListView->setModel(&this->appModel);

    Application::GetApplication()->appManager->LoadApps();
    for (auto app : Application::GetApplication()->appManager->apps)
    {
        this->appModel.addApp(app.second->inCategories, app.second);
        //DEBUG_MSG << app.first << ": ";
        //LogAppUpdateInfo(app.second->updateInfo);
    }
    //ui->gamesListWidget->sortItems();
}

void MainWindow::openSettings() {
   
}

void MainWindow::UpdatePlayButton()
{

    disconnect(currentPlayBtnAction);

    ui->playButton->setEnabled(true);

    if (this->selectedApp->state->AppRunning)
    {
        ui->playButton->setText("Stop");
        currentPlayBtnAction = connect(ui->playButton, &QPushButton::clicked, this, &MainWindow::stopClicked);
        return;
    }

    if (this->selectedApp->state->UpdateRunning) {
        ui->playButton->setText("Updating");
        ui->playButton->setEnabled(false);
        return;
    }

    if (this->selectedApp->state->FullyInstalled) {
        ui->playButton->setText("Play");
        currentPlayBtnAction = connect(ui->playButton, &QPushButton::clicked, this, &MainWindow::playClicked);
        return;
    }

    if (this->selectedApp->state->UpdateRequired) {
        ui->playButton->setText("Update");
        currentPlayBtnAction = connect(ui->playButton, &QPushButton::clicked, this, &MainWindow::updateClicked);
        return;
    } 

    if (this->selectedApp->state->Uninstalled) {
        ui->playButton->setText("Install");
        currentPlayBtnAction = connect(ui->playButton, &QPushButton::clicked, this, &MainWindow::installClicked);
        return;
    }

    ui->playButton->setText(QString("AppState %1").arg(this->selectedApp->state->state));
}

void MainWindow::currentItemChanged(const QModelIndex &current, const QModelIndex &previous) {
    TreeItem *data = qvariant_cast<TreeItem*>(appModel.data(current, 0));
    if (data->type == TreeItemType::k_ETreeItemTypeApp) {
        this->selectedApp = qvariant_cast<App*>(data->value);
        DEBUG_MSG << "Item changed to " << this->selectedApp->name << " with appid " << this->selectedApp->appid << std::endl;
        ui->currentGameLabel->setText(QString::fromStdString(this->selectedApp->name));
        UpdatePlayButton();
    }
}

void MainWindow::installClicked() {
    auto installDialog = new InstallGameDialog(this, this->selectedApp);
    installDialog->show();
}

void MainWindow::stopClicked() {
    //TODO: give user option to force/not
    this->selectedApp->Kill(true);
}

void MainWindow::updateClicked() {
    
}

void MainWindow::playClicked() {

    //Global_SteamClientMgr->ClientAppManager->AddLibraryFolder("/home/shared/Games/SteamLibrary");
    //Global_SteamClientMgr->ClientAppManager->AddLibraryFolder("/mnt/deathclaw/SteamLibrary");

    std::vector<LaunchOption> workingLaunchOptions = this->selectedApp->GetFilteredLaunchOptions();
    DEBUG_MSG << "Working launch options (" << workingLaunchOptions.size() << "):" << std::endl;
    for (auto &&i : workingLaunchOptions)
    {
        DEBUG_MSG << i.index << " " << i.name << std::endl;
    }

    if (workingLaunchOptions.size() <= 0) {
        DEBUG_MSG << "No launch options available." << std::endl;
        QMessageBox msgBox;
        msgBox.setWindowTitle("Failed to launch");
        msgBox.setText(QString("Game %1 doesn't have valid launch options. Is Proton enabled?").arg(QString::fromStdString(this->selectedApp->name)));
        msgBox.setIcon(QMessageBox::Icon::Critical);
        msgBox.exec();
    }

    connect(this->selectedApp, &App::AppLaunchOrUpdateError, this, &MainWindow::launchFailed);
    if (workingLaunchOptions.size() == 1) {
        DEBUG_MSG << "One launch option matches. Launching directly." << std::endl;
        this->selectedApp->Launch(workingLaunchOptions[0]);
    }
    if (workingLaunchOptions.size() > 1) {
        LaunchOptionsDialog *diag = new LaunchOptionsDialog(this, workingLaunchOptions);
        connect(diag, &LaunchOptionsDialog::OnCancelled, this, &MainWindow::launchOptionsDialog_cancelled, Qt::ConnectionType::SingleShotConnection);
        connect(diag, &LaunchOptionsDialog::OnOptionSelected, this, &MainWindow::launchOptionsDialog_optionSelected, Qt::ConnectionType::SingleShotConnection);
        diag->show();
    }
}

void MainWindow::launchOptionsDialog_cancelled() {
    DEBUG_MSG << "launch cancelled" << std::endl;
}

void MainWindow::launchOptionsDialog_optionSelected(LaunchOption opt) {
    this->selectedApp->Launch(opt);
}

void CommonAppUpdateErrorHandler(App* app, EAppUpdateError err, std::string title, std::string action) {
    QMessageBox msgBox;
    msgBox.setWindowTitle(QString::fromStdString(title));
    msgBox.setText(QString("An error occurred while %1 %2: %3").arg(QString::fromStdString(action), QString::fromStdString(app->name), QString::fromStdString(ErrMsgUtils::GetErrorMessageFromEAppUpdateError(err))));
    msgBox.setIcon(QMessageBox::Icon::Critical);
    msgBox.exec();
}

void MainWindow::launchFailed(EAppUpdateError err) {
    std::cerr << "err is " << err << std::endl;
    if (err == 0)
        return;
    CommonAppUpdateErrorHandler(qobject_cast<App*>(sender()), err, "Launch failed", "launching");
    disconnect(qobject_cast<App *>(sender()), &App::AppLaunchOrUpdateError, this, &MainWindow::launchFailed);
}

void MainWindow::uninstallFailed(EAppUpdateError err) {
    if (err == 0)
        return;
    CommonAppUpdateErrorHandler(qobject_cast<App*>(sender()), err, "Uninstall failed", "uninstalling");
    disconnect(qobject_cast<App *>(sender()), &App::AppLaunchOrUpdateError, this, &MainWindow::uninstallFailed);
}

void MainWindow::moveFailed(EAppUpdateError err) {
    if (err == 0)
        return;
    CommonAppUpdateErrorHandler(qobject_cast<App*>(sender()), err, "Moving failed", "moving");
    disconnect(qobject_cast<App *>(sender()), &App::AppLaunchOrUpdateError, this, &MainWindow::moveFailed);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_enableDownloadsBox_stateChanged(int arg1)
{
    Global_SteamClientMgr->ClientAppManager->SetDownloadingEnabled((bool)arg1);
}

void MainWindow::on_allowDownloadsWhilePlayingBox_stateChanged(int arg1)
{
    Global_SteamClientMgr->ClientAppManager->SetAllowDownloadsWhileAnyAppRunning((bool)arg1);
}



void MainWindow::on_settingsButton_clicked()
{
    SettingsWindow *settingsWindow = new SettingsWindow(this, this->selectedApp);
    settingsWindow->show();
}

void MainWindow::on_DEBUGenableProtonChecck_stateChanged(int arg1)
{
    Global_SteamClientMgr->ClientCompat->EnableCompat((bool)arg1);
}
