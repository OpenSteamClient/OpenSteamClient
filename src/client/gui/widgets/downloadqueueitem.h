#ifndef DOWNLOADQUEUEITEM_H
#define DOWNLOADQUEUEITEM_H

#include <QWidget>
#include "../downloads/downloadinfothread.h"
#include "../../interop/includesteamworks.h"

class DownloadItem;

namespace Ui {
class DownloadQueueItem;
}

class DownloadQueueItem : public QWidget
{
    Q_OBJECT

public:
    explicit DownloadQueueItem(QWidget *parent = nullptr);
    void SetItemData(DownloadItem *data);
    void SetPositionSectionHidden(bool visible);
    ~DownloadQueueItem();
public slots:
    void UpdateUpdateInfo(AppUpdateInfo_s);

signals:
    void RequestUpdate(AppId_t);

private slots:
    void on_updateButton_clicked();

private:
    bool updateButtonState = false;
    DownloadItem *data;
    Ui::DownloadQueueItem *ui;
};

#endif // DOWNLOADQUEUEITEM_H
