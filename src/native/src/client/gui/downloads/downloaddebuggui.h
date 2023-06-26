#ifndef DOWNLOADDEBUGGUI_H
#define DOWNLOADDEBUGGUI_H

#include <QDialog>

namespace Ui {
class DownloadDebugGui;
}

class DownloadManager;

class DownloadDebugGui : public QDialog
{
    Q_OBJECT

public:
    explicit DownloadDebugGui(QWidget *parent = nullptr);
    ~DownloadDebugGui();

private:
    void ReloadDownloads();

private slots:
    void on_pushButton_clicked();

    void on_pushButton_2_clicked();

    void on_moveToQueue_clicked();

    void on_moveToUnscheduled_clicked();

    void on_moveToScheduled_clicked();

    void on_upBtn_clicked();

    void on_downBtn_clicked();

private:
    Ui::DownloadDebugGui *ui;
    DownloadManager *mgr;
};

#endif // DOWNLOADDEBUGGUI_H
