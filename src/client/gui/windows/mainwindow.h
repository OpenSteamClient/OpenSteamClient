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

namespace Ui
{
    class MainWindow;
}

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
    void changeAccount();
    
    void currentItemChanged(const QModelIndex &current, const QModelIndex &previous);
    void launchOptionsDialog_cancelled();
    void launchOptionsDialog_optionSelected(LaunchOption opt);

    void on_enableDownloadsBox_stateChanged(int arg1);

    void on_allowDownloadsWhilePlayingBox_stateChanged(int arg1);

    void on_settingsButton_clicked();

    void on_DEBUGenableProtonChecck_stateChanged(int arg1);

    // Error handling
    void launchFailed(EAppUpdateError err);
    void uninstallFailed(EAppUpdateError err);
    void moveFailed(EAppUpdateError err);

private:
    AppModel appModel;
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
};

#endif // MAINWINDOW_H
