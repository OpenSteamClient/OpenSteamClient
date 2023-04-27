#ifndef DOWNLOADQUEUEITEM_H
#define DOWNLOADQUEUEITEM_H

#include <QWidget>
#include "../downloads/downloadinfothread.h"
#include "../../interop/includesteamworks.h"

struct DownloadQueueItemData
{
    AppId_t appid;
    std::string name;
    AppUpdateInfo_s initialUpdateInfo;
};

namespace Ui {
class DownloadQueueItem;
}

class DownloadQueueItem : public QWidget
{
    Q_OBJECT

public:
    explicit DownloadQueueItem(QWidget *parent = nullptr);
    void SetItemData(DownloadQueueItemData data);
    ~DownloadQueueItem();
public slots:
    void UpdateUpdateInfo(AppUpdateInfo_s);

signals:
    void RequestUpdate(AppId_t);

private slots:
    void on_updateButton_clicked();

private:
    DownloadQueueItemData data;
    Ui::DownloadQueueItem *ui;
};

#endif // DOWNLOADQUEUEITEM_H
