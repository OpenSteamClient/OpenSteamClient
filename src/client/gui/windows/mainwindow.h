#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QListWidgetItem>
#include "../../interop/launchoption.h"
#include "../downloads/downloadinfothread.h"
#include <QGraphicsView>
#include <QGraphicsScene>
#include <QGraphicsPixmapItem>
#include <QGraphicsSimpleTextItem>
#include "../widgets/clickableqwidget.h"
#include "../gamelistview/appmodel.h"
#include "../widgets/downloadqueueitem.h"
#include <map>
#include "../widgets/dynamicwebviewwidget.h"
#include <QPushButton>
#include <QSortFilterProxyModel>
#include "../widgets/filterspopup.h"

namespace Ui
{
    class MainWindow;
}

enum TabType {
    k_EWebviewTabTypeStore,
    k_EWebviewTabTypeCommunity,
    k_EWebviewTabTypeProfile,
    k_EWebviewTabTypeOther,
    k_ETabTypeLibrary,
    k_ETabTypeDownloads,
    k_ETabTypeConsole,
    k_ETabTypeInvalid
};


class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow();
private slots:

    // Play button actions
    void playClicked();
    void installClicked();
    void stopClicked();
    void updateClicked();

    // Downloads page
    void updateDownloadSpeed(DownloadSpeedInfo);
    void currentDownloadingAppChanged(AppId_t);

    // Page switching
    void openDownloads();
    
    // Dialog pop-upping
    void openSettings();

    // Logout
    void changeAccount();
    void signOut();

    // Web view
    void GotoWebviewTab(TabType type, QUrl url = QUrl());
    void FreeWebviewTab(TabType type);

    // Context menus
    void headerButtonCustomMenuRequested(QPoint pos);

    void currentItemChanged(const QModelIndex &current, const QModelIndex &previous);
    void launchOptionsDialog_cancelled();
    void launchOptionsDialog_optionSelected(LaunchOption opt);

    void on_settingsButton_clicked();

    // Error handling
    void launchFailed(EAppUpdateError err);
    void uninstallFailed(EAppUpdateError err);
    void moveFailed(EAppUpdateError err);

    void on_storeButton_clicked();

    void on_libraryButton_clicked();

    void on_communityButton_clicked();

    void on_profileButton_clicked();

    void on_consoleButton_clicked();

    void on_tabWidget_currentChanged(int index);

    void on_filterButton_clicked(bool checked);

    void on_gameSearchBar_textChanged(const QString &newText);

    void on_pauseDownloadButton_clicked();

    void on_cellIdDebugBox_editingFinished();

    void sortingFinished();

    // Game web buttons
    void on_storePageButton_clicked();
    void on_supportButton_clicked();
    void on_communityHubButton_clicked();
    void on_discussionsButton_clicked();
    void on_guidesButton_clicked();
    void on_workshopButton_clicked();

private:
    AppModel appModel;
    QSortFilterProxyModel *proxyModel;
    FiltersPopup *filtersPopup;
    // AppId,  {pos in queue, queue info}
    std::map<AppId_t, std::pair<int, DownloadQueueItem*>> downloadQueueItems;
    App *selectedApp;
    Ui::MainWindow *ui;
    void LoadApps();
    void UpdatePlayButton();
    void UpdateBottomDownloadsBar();
    void UpdateDownloadQueue();
    void UpdateAppState(AppId_t);
    QMetaObject::Connection currentPlayBtnAction;
    QGraphicsScene* scene;
    QGraphicsView* view;
    QGraphicsPixmapItem* heroItem;
    QGraphicsPixmapItem* logoItemIfImg;
    QGraphicsSimpleTextItem *logoItemIfText;
    ClickableQWidget *bottomDownloadSection;
    std::map<TabType, QWidget*> webviewPages;
    std::map<TabType, QPushButton *> tabTypeToHeaderButtonMap;
    // Map of the context menu entries of header buttons.
    // QVariant can be a QUrl or QWidget*
    std::map<TabType, std::list<std::pair<std::string, QVariant>>> tabTypeToHeaderButtonContextMenuEntriesMap;
    std::string currentUserProfileID;
};

#endif // MAINWINDOW_H
