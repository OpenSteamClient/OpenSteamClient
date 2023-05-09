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
#include "appsettingswindow.h"

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
    connect(quitAndRestoreValveSteamAction, &QAction::triggered, Application::GetApplication(), &Application::quitAppAndRestoreValveSteam);
    connect(changeAccountAct, &QAction::triggered, this, &MainWindow::changeAccount);
    connect(signOutAct, &QAction::triggered, this, &MainWindow::signOut);
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
    
    //TODO: these should be somewhere else (this enables downloading)
    on_pauseDownloadButton_clicked();

    LoadApps();

    connect(&this->appModel, &AppModel::sortingFinishedd, this, &MainWindow::sortingFinished);
    connect(ui->appsListView->selectionModel(), &QItemSelectionModel::currentChanged, this, &MainWindow::currentItemChanged);
    connect(Global_ThreadController->downloadInfoThread, &DownloadInfoThread::DownloadSpeedUpdate, this, &MainWindow::updateDownloadSpeed);
    connect(Global_ThreadController->downloadInfoThread, &DownloadInfoThread::DownloadingAppChange, this, &MainWindow::currentDownloadingAppChanged);

    // Start and register the thread at this point so we don't miss signals
    Global_ThreadController->initThread(Global_ThreadController->downloadInfoThread);
    Global_ThreadController->StartThread(Global_ThreadController->downloadInfoThread);

    // This turns the bottom section downloads QWidget into a clickable one
    this->bottomDownloadSection = new ClickableQWidget(this);
    ui->bottomSection->insertWidget(ui->bottomSection->indexOf(ui->bottomDownloadSection), this->bottomDownloadSection);
    this->bottomDownloadSection->ReplaceExistingQWidget(ui->bottomDownloadSection);
    connect(this->bottomDownloadSection, &ClickableQWidget::clicked, this, &MainWindow::openDownloads);

    // Sets the app list view to be aligned nicely
    ui->appsListView->setIndentation(24);

    // Hide the tabs of the QTabWidget
    QTabBar *tabBar = ui->tabWidget->findChild<QTabBar *>();
    tabBar->setEnabled(false);
    tabBar->hide();

    ui->gameDetailsFrame->setVisible(false);

    // Set the username where it needs to be visible
    std::string username;
    username = std::string(Global_SteamClientMgr->ClientFriends->GetPersonaName());
    ui->profileButton->setText(QString::fromStdString(username));
    ui->usernameLabel->setText(QString::fromStdString(username));
    

    // Load wallet and display
    {
        bool hasWallet;
        CAmount amount; 
        CAmount amountPending;
        bool success = Global_SteamClientMgr->ClientUser->BGetWalletBalance( &hasWallet, &amount, &amountPending );
        if(success && hasWallet)
        {
            float balance = static_cast<float>(static_cast<float>(amount.m_nAmount) * 0.01);
            float balancePending = static_cast<float>(static_cast<float>(amountPending.m_nAmount) * 0.01);
            if (balance > 0 || balancePending > 0) {
                ui->walletInfoWidget->setVisible(true);
                QString baseStr = QString("Wallet: %1€").arg(balance);
                if (balancePending > 0) {
                    baseStr = baseStr.append(" (pending: %1€)").arg(balancePending);
                }
                ui->walletMoneyLabel->setText(baseStr);
            } else {
                ui->walletInfoWidget->setVisible(false);
            }
        }
    }

    filtersPopup = new FiltersPopup(&this->appModel, this);
    filtersPopup->hide();

    tabTypeToHeaderButtonMap = {
        {k_EWebviewTabTypeStore, ui->storeButton},
        {k_EWebviewTabTypeCommunity, ui->communityButton},
        {k_EWebviewTabTypeProfile,  ui->profileButton},
        {k_ETabTypeConsole, ui->consoleButton},
        {k_ETabTypeDownloads, nullptr},
        {k_ETabTypeLibrary, ui->libraryButton}
    };

    tabTypeToHeaderButtonContextMenuEntriesMap = {
        {k_EWebviewTabTypeStore, std::list<std::pair<std::string, QVariant>> {
            {"if_webviewloaded:Unload", QVariant::fromValue<QWidget*>(nullptr)},
            {"if_webviewloaded:separator", QVariant::fromValue<QWidget*>(nullptr)},
            {"Featured", QUrl("https://store.steampowered.com")},
            {"Discovery Queue", QUrl("https://store.steampowered.com/explore")},
            {"Wishlist", QUrl("https://store.steampowered.com/wishlist")},
            {"Points Shop", QUrl("https://store.steampowered.com/points/shop")},
            {"News", QUrl("https://store.steampowered.com/news")},
            {"Stats", QUrl("https://store.steampowered.com/stats")}
        }},
        {k_EWebviewTabTypeCommunity, std::list<std::pair<std::string, QVariant>> {
            {"if_webviewloaded:Unload", QVariant::fromValue<QWidget*>(nullptr)},
            {"if_webviewloaded:separator", QVariant::fromValue<QWidget*>(nullptr)},
            {"Home", QUrl("https://steamcommunity.com/home")},
            {"Discussions", QUrl("https://steamcommunity.com/discussions")},
            {"Workshop", QUrl("https://steamcommunity.com/workshop")},
            {"Market", QUrl("https://steamcommunity.com/market")},
            {"Broadcasts", QUrl("https://steamcommunity.com/?subsection=broadcasts")}
        }},
        {k_EWebviewTabTypeProfile, std::list<std::pair<std::string, QVariant>> {
            {"if_webviewloaded:Unload", QVariant::fromValue<QWidget*>(nullptr)},
            {"if_webviewloaded:separator", QVariant::fromValue<QWidget*>(nullptr)},
            {"Activity", QUrl("https://steamcommunity.com/my/home/")},
            {"Profile", QUrl(QString("https://steamcommunity.com/profiles/%1").arg(Application::GetApplication()->currentUserSteamID))},
            {"Friends", QUrl("https://steamcommunity.com/my/friends")},
            {"Groups", QUrl("https://steamcommunity.com/my/groups")},
            {"Screenshots", QUrl("https://steamcommunity.com/my/screenshots")},
            {"Badges", QUrl("https://steamcommunity.com/my/badges")},
            {"Inventory", QUrl("https://steamcommunity.com/my/inventory")},
        }},
        {k_ETabTypeConsole, std::list<std::pair<std::string, QVariant>>()},
        {k_ETabTypeDownloads, std::list<std::pair<std::string, QVariant>>()},
        {k_ETabTypeLibrary, std::list<std::pair<std::string, QVariant>> {
            {"Home", QVariant::fromValue<QWidget*>(ui->gamesTab)},
            {"separator", QVariant::fromValue<QWidget*>(nullptr)},
            {"Downloads", QVariant::fromValue<QWidget*>(ui->downloadsTab)}
        }}
    };

    // Custom context menus for the header buttons
    for (auto &&i : ui->navButtonsWidget->findChildren<QPushButton*>())
    {
        i->setContentsMargins(0, 0, 0, 0);
        i->setStyleSheet("padding: 0px 0px 0px 0px;");
        i->setContextMenuPolicy(Qt::CustomContextMenu);
        connect(i, &QPushButton::customContextMenuRequested, this, &MainWindow::headerButtonCustomMenuRequested);
    }

    // This was originally inteded to be an in-window popup that appears right below the filters button but it wasn't possible easily
    ui->filtersPopupContainer->addWidget(filtersPopup);

    // Debug stuff
    ui->cellIdDebugBox->setText(QString::fromStdString(std::to_string(Global_SteamClientMgr->ClientUtils->GetCellID())));

    // Global_SteamClientMgr->ClientAppManager->AddLibraryFolder("/home/shared/Games/SteamLibrary");
    // Global_SteamClientMgr->ClientAppManager->AddLibraryFolder("/mnt/deathclaw/SteamLibrary");

}

void MainWindow::headerButtonCustomMenuRequested(QPoint pos)
{
    QWidget *senderObj = qobject_cast<QWidget *>(sender());
    TabType type = k_ETabTypeInvalid;
    for (auto &&i : tabTypeToHeaderButtonMap)
    {
        if (i.second == nullptr)
            continue;
            
        if (i.second->objectName() == senderObj->objectName())
        {
            type = i.first;
            break;
        }
    }
    
    if (type == k_ETabTypeInvalid) {
        DEBUG_MSG << "[MainWindow] Custom context menu requested for " << senderObj->objectName().toStdString() << " but it couldn't be mapped to any TabType" << std::endl;
        return;
    }

    QMenu *menu = new QMenu(this);
    for (auto &&i : tabTypeToHeaderButtonContextMenuEntriesMap.at(type))
    {
        if (i.first.starts_with("if_webviewloaded:")) {
            bool isLoaded = webviewPages.contains(type);
            if (!isLoaded)
            {
                continue;
            }
            i.first = i.first.substr(strlen("if_webviewloaded:"));
        }

        if (i.first == "separator") {
            menu->addSeparator();
            continue;
        }

        if (i.first == "Unload") {
            QAction *act = new QAction(QString::fromStdString(i.first), this);
            connect(act, &QAction::triggered, this, [this, act, type]()
            {
                QMessageBox *box = new QMessageBox(this);
                box->setWindowTitle("Notice");
                box->setText("Unloading WebViews is currently not supported.");
                box->setIcon(QMessageBox::Icon::Information);
                box->exec();
                //TODO: freeing webviews 
                // this->FreeWebviewTab(type);
            });
            menu->addAction(act);
            continue;
        }

        QAction *act = new QAction(QString::fromStdString(i.first), this);
        if (i.second.canConvert<QUrl>()) {
            connect(act, &QAction::triggered, this, [this, act, type, i]()
            { 
                this->GotoWebviewTab(type, i.second.value<QUrl>()); 
            });
        } else if (i.second.canConvert<QWidget *>()) {
            connect(act, &QAction::triggered, this, [this, act, type, i]()
            { 
                QWidget *page = i.second.value<QWidget *>();
                if (page == nullptr)
                {
                    return;
                }
                    
                this->ui->tabWidget->setCurrentWidget(page); 
            });
        }
        
        menu->addAction(act);
    }
    
    menu->popup(senderObj->mapToGlobal(pos));
}

// Goes to a Webview tab
// Creates it if it doesn't exist and webviews are allowed.
void MainWindow::GotoWebviewTab(TabType type, QUrl url) {
    if (webviewPages.contains(type) && webviewPages.at(type)->findChild<DynamicWebViewWidget*>()->isWebViewLoaded) {
        if (!url.isEmpty()) {
            DEBUG_MSG << "[MainWindow] loading url " << url.toString().toStdString() << std::endl;
            webviewPages.at(type)->findChild<DynamicWebViewWidget *>()->LoadURL(url);
        }
        ui->tabWidget->setCurrentIndex(ui->tabWidget->indexOf(webviewPages.at(type)));
        return;
    }

    QUrl urlToUse;
    switch (type)
    {
    case k_EWebviewTabTypeStore:
        urlToUse = QUrl("https://store.steampowered.com");
        break;
    case k_EWebviewTabTypeCommunity:
        urlToUse = QUrl("https://steamcommunity.com");
        break;
    case k_EWebviewTabTypeProfile:
        urlToUse = QUrl(QString("https://steamcommunity.com/profiles/%1").arg(Application::GetApplication()->currentUserSteamID));
        break;

    default:
        return;
    }

    if (!url.isEmpty()) {
        urlToUse = url;
    }

    // Creates a new tab
    QWidget *newTab = new QWidget();
    newTab->setLayout(new QGridLayout());
    newTab->layout()->setContentsMargins(0, 0, 0, 0);

    // Creates the dynamic web view and tries to load it
    DynamicWebViewWidget *webviewwidget = new DynamicWebViewWidget(newTab);
    webviewwidget->LoadWebView();
    DEBUG_MSG << "[MainWindow] loading url " << urlToUse.toString().toStdString() << " for type " << type << std::endl;
    webviewwidget->LoadURL(urlToUse);
    newTab->layout()->addWidget(webviewwidget);
    webviewPages.insert({type, newTab});
    if (this->tabTypeToHeaderButtonMap.contains(type)) {
        QPushButton *btn = this->tabTypeToHeaderButtonMap.at(type);
        QPalette palette = btn->palette();
        palette.setColor(btn->foregroundRole(), Qt::green);
        btn->setPalette(palette);
    }

    // Sets the active tab to the new one
    ui->tabWidget->setCurrentIndex(ui->tabWidget->addTab(newTab, QString("WebviewTab_%1").arg(QString::fromStdString(std::to_string(type)))));
}

void MainWindow::FreeWebviewTab(TabType type) {
    if (!webviewPages.contains(type)) {
        return;
    }
    DynamicWebViewWidget *widget = webviewPages.at(type)->findChild<DynamicWebViewWidget *>();
    widget->UnloadWebView();

    if (this->tabTypeToHeaderButtonMap.contains(type)) {
        QPushButton *btn = this->tabTypeToHeaderButtonMap.at(type);
        QPalette palette = btn->palette();
        // No way to get default QStyle so just use the MainWindow's one
        palette.setColor(btn->foregroundRole(), this->palette().color(btn->foregroundRole()));
        btn->setPalette(palette);
    }
}

void MainWindow::openDownloads() {
    ui->tabWidget->setCurrentIndex(ui->tabWidget->indexOf(ui->downloadsTab));
}

void LogAppUpdateInfo(AppUpdateInfo_s appUpdateInfo) {
      DEBUG_MSG << "AppUpdateInfo_s: { m_timeUpdateStart=" << appUpdateInfo.m_timeUpdateStart
          << ", m_eAppUpdateState=" << appUpdateInfo.m_eAppUpdateState
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
          << ", m_someError=" << appUpdateInfo.m_someError
          << ", m_uUnk7=" << appUpdateInfo.m_uUnk7
          << ", m_uUnk8=" << appUpdateInfo.m_uUn8
          << ", m_targetBuildID=" << appUpdateInfo.m_targetBuildID
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
    DEBUG_MSG << "[MainWindow] current downloading app changed" << std::endl;
    if (appid != 0)
    {
        char name[512];
        Global_SteamClientMgr->ClientApps->GetAppData(appid, "common/name", name, sizeof(name));
        ui->bottomDownloadingLabel->setText(QString::fromStdString(std::string(name)));
    }
    else
    {
        ui->bottomDownloadingLabel->setText(QString("No download"));
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
        DEBUG_MSG << "[MainWindow] " << currentAppId << ": ";
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
        ui->bottomDownloadingProgress->setValue(percentDownloaded);
    }
}

void MainWindow::changeAccount() {
    hide();
    Application::GetApplication()->loginWindow->logoutAndShowWindow();
    delete this;
}

void MainWindow::signOut() {
    hide();
    Application::GetApplication()->loginWindow->logoutAndShowWindow(true);
    delete this;
}

void MainWindow::LoadApps() {
    ui->appsListView->setItemDelegate(new AppDelegate);
    ui->appsListView->setModel(&this->appModel);

    Application::GetApplication()->appManager->LoadApps();
    for (auto app : Application::GetApplication()->appManager->apps)
    {
        // Filter these out, they are not useful for end users
        switch (app.second->type)
        {
        case k_EAppTypeConfig:
        case k_EAppTypeDepotonly:
        case k_EAppTypeDlc:
        case k_EAppTypeDriver:
        // This is all the trailers on the store pages
        case k_EAppTypeMedia:
        case k_EAppTypeComic:
        case k_EAppTypeFranchise:
        case k_EAppTypeGuide:
        case k_EAppTypeHardware:
        case k_EAppTypeSeries:
        // We don't support video playback
        case k_EAppTypeVideo:
            continue;

        default:
            this->appModel.addApp(app.second->inCategories, app.second, true);
            break;
        }
    }

    this->appModel.filter();
    // Expand all categories
    // TODO: make this a setting?
    ui->appsListView->expandToDepth(1);
}

void MainWindow::openSettings() {
    auto settings = new AppSettingsWindow(this);
    settings->show();
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
    TreeItem *data = qvariant_cast<TreeItem *>(appModel.data(current, 0));

    // Something is sometimes causing this to be a nullptr sometimes. What?
    if (data == nullptr) {
        return;
    }

    if (data->type == TreeItemType::k_ETreeItemTypeApp) {
        this->selectedApp = qvariant_cast<App*>(data->value);
        DEBUG_MSG << "[MainWindow] Selected App changed to " << this->selectedApp->name << " with appid " << this->selectedApp->appid << std::endl;
        ui->currentGameLabel->setText(QString::fromStdString(this->selectedApp->name));
        UpdatePlayButton();
        if (!ui->gameDetailsFrame->isVisible()) {
            ui->gameDetailsFrame->setVisible(true);
        }
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

    std::vector<LaunchOption> workingLaunchOptions = this->selectedApp->GetFilteredLaunchOptions();
    DEBUG_MSG << "[MainWindow] Working launch options (" << workingLaunchOptions.size() << "):" << std::endl;
    for (auto &&i : workingLaunchOptions)
    {
        DEBUG_MSG << i.index << " " << i.name << std::endl;
    }

    if (workingLaunchOptions.size() <= 0) {
        DEBUG_MSG << "[MainWindow] No launch options available." << std::endl;
        QMessageBox msgBox;
        msgBox.setWindowTitle("Failed to launch");
        msgBox.setText(QString("Game %1 doesn't have valid launch options. Is Proton enabled?").arg(QString::fromStdString(this->selectedApp->name)));
        msgBox.setIcon(QMessageBox::Icon::Critical);
        msgBox.exec();
    }

    connect(this->selectedApp, &App::AppLaunchOrUpdateError, this, &MainWindow::launchFailed);
    if (workingLaunchOptions.size() == 1) {
        DEBUG_MSG << "[MainWindow] One launch option matches. Launching directly." << std::endl;
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
    DEBUG_MSG << "[MainWindow] launch cancelled" << std::endl;
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

void MainWindow::on_settingsButton_clicked()
{
    SettingsWindow *settingsWindow = new SettingsWindow(this, this->selectedApp);
    settingsWindow->show();
}

void MainWindow::on_storeButton_clicked()
{
    GotoWebviewTab(k_EWebviewTabTypeStore);
}

void MainWindow::on_libraryButton_clicked()
{
    ui->tabWidget->setCurrentWidget(ui->gamesTab);
}

void MainWindow::on_communityButton_clicked()
{
    GotoWebviewTab(k_EWebviewTabTypeCommunity);
}


void MainWindow::on_profileButton_clicked()
{
    GotoWebviewTab(k_EWebviewTabTypeProfile);
}


void MainWindow::on_consoleButton_clicked()
{
    ui->tabWidget->setCurrentWidget(ui->consoleTab);
}

void MainWindow::on_tabWidget_currentChanged(int index)
{
    QWidget *tab = ui->tabWidget->currentWidget();
    TabType tabType = k_ETabTypeInvalid;

    for (auto &&i : webviewPages)
    {   
        if (i.second == tab) {
            tabType = i.first;
            break;
        }
    }

    if (tabType == k_ETabTypeInvalid) {
        if (tab == ui->gamesTab) {
            tabType = k_ETabTypeLibrary;
        } else if (tab == ui->consoleTab) {
            tabType = k_ETabTypeConsole;
        } else if (tab == ui->downloadsTab) {
            tabType = k_ETabTypeDownloads;
        }
    }
    
    if (tabType == k_ETabTypeInvalid) {
        DEBUG_MSG << "[MainWindow] Failed to determine tab type for tab at index " << index << std::endl;
        return;
    }

    // Clear the existing selection
    for (auto &&i : ui->navButtonsWidget->findChildren<QPushButton*>())
    {
        // Need more advanced logic if we style things better in the future
        QFont origFont = i->font();
        origFont.setBold(false);
        i->setFont(origFont);
    }

    QWidget *tabButton = nullptr;
    if (tabTypeToHeaderButtonMap.contains(tabType)) {
        tabButton = tabTypeToHeaderButtonMap.at(tabType);
    }

    if (tabButton == nullptr) {
        DEBUG_MSG << "[MainWindow] Tab type " << tabType << " did not map to any button" << std::endl;
        return;
    }
    QFont origFont = tabButton->font();
    origFont.setBold(true);
    tabButton->setFont(origFont);
}

void MainWindow::on_filterButton_clicked(bool checked)
{
    filtersPopup->setVisible(checked);
}

void MainWindow::on_gameSearchBar_textChanged(const QString &newText)
{
    appModel.setNameContainsFilter(newText.toStdString());
}

void MainWindow::on_pauseDownloadButton_clicked()
{
    if (Global_SteamClientMgr->ClientAppManager->BIsDownloadingEnabled()) {
        Global_SteamClientMgr->ClientAppManager->SetDownloadingEnabled(false);
        ui->pauseDownloadButton->setText("Continue");
    } else {
        Global_SteamClientMgr->ClientAppManager->SetDownloadingEnabled(true);
        ui->pauseDownloadButton->setText("Pause");
    }
}


void MainWindow::on_cellIdDebugBox_editingFinished()
{
    Global_SteamClientMgr->ClientUser->SetCellID(std::stol(ui->cellIdDebugBox->text().toStdString()));
}

void MainWindow::sortingFinished() {
    //TODO: the model shouldn't reset completely, it should somehow calculate the data change, for now do this
    ui->appsListView->expandToDepth(1);
}
void MainWindow::on_storePageButton_clicked()
{
    GotoWebviewTab(k_EWebviewTabTypeStore, QUrl(QString("https://store.steampowered.com/app/%1").arg(this->selectedApp->appid)));
}


void MainWindow::on_supportButton_clicked()
{   
    // Should we add a separate tab for this? Let's use the store tab for now.
    GotoWebviewTab(k_EWebviewTabTypeStore, QUrl(QString("https://help.steampowered.com/en/wizard/HelpWithGame/?appid=%1").arg(this->selectedApp->appid)));
}


void MainWindow::on_communityHubButton_clicked()
{
    GotoWebviewTab(k_EWebviewTabTypeCommunity, QUrl(QString("https://steamcommunity.com/app/%1").arg(this->selectedApp->appid)));
}

void MainWindow::on_discussionsButton_clicked()
{
    GotoWebviewTab(k_EWebviewTabTypeCommunity, QUrl(QString("https://steamcommunity.com/app/%1/discussions").arg(this->selectedApp->appid)));
}


void MainWindow::on_guidesButton_clicked()
{
    GotoWebviewTab(k_EWebviewTabTypeCommunity, QUrl(QString("https://steamcommunity.com/app/%1/guides").arg(this->selectedApp->appid)));
}


void MainWindow::on_workshopButton_clicked()
{
    GotoWebviewTab(k_EWebviewTabTypeCommunity, QUrl(QString("https://steamcommunity.com/app/%1/workshop").arg(this->selectedApp->appid)));
}

