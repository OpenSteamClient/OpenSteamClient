#include "downloadqueueitem.h"
#include "ui_downloadqueueitem.h"
#include <QString>
#include "../../interop/downloads.h"

DownloadQueueItem::DownloadQueueItem(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::DownloadQueueItem)
{
    ui->setupUi(this);
    ui->positionButtonsLayout->setSpacing(0);
    ui->positionButtonsLayout->setContentsMargins(0, 0, 0, 0);
}

void DownloadQueueItem::SetItemData(DownloadItem *data) {
    this->data = data;
    ui->gameName->setText(QString::fromStdString(data->app->name));
    ui->indexLabel->setText(QString::fromStdString(std::to_string(data->queueIndex)));
    SetPositionSectionHidden(data->queued);

    UpdateUpdateInfo(data->app->updateInfo);
}

void DownloadQueueItem::UpdateUpdateInfo(AppUpdateInfo_s updateInfo)  
{
    std::string totalDownloaded = std::to_string((float)(updateInfo.m_unBytesDownloaded / (float)1024 / (float)1024 / (float)1024)).substr(0, 4);
    std::string totalToDownload = std::to_string((float)(updateInfo.m_unBytesToDownload / (float)1024 / (float)1024 / (float)1024)).substr(0, 4);
    std::string totalProcessed = std::to_string((float)(updateInfo.m_unBytesProcessed / (float)1024 / (float)1024 / (float)1024)).substr(0, 4);
    std::string totalToProcess = std::to_string((float)(updateInfo.m_unBytesToProcess / (float)1024 / (float)1024 / (float)1024)).substr(0, 4);
    QString totalUnit = "GB";

    ui->downloaded->setText(QString("%1 %2 / %3 %2").arg(QString::fromStdString(totalDownloaded), totalUnit, QString::fromStdString(totalToDownload)));
    ui->installed->setText(QString("%1 %2 / %3 %2").arg(QString::fromStdString(totalProcessed), totalUnit, QString::fromStdString(totalToProcess)));

    double divideResult = (double)updateInfo.m_unBytesDownloaded / (double)updateInfo.m_unBytesToDownload;
    int percentDownloaded = static_cast<int>(divideResult * 100);
    ui->progressBar->setValue(percentDownloaded);
}

void DownloadQueueItem::SetPositionSectionHidden(bool visible) {
    for (auto &&i : ui->positionButtonsLayout->findChildren<QWidget*>())
    {
        i->setVisible(visible);
    }
}

DownloadQueueItem::~DownloadQueueItem()
{
    delete ui;
}

void DownloadQueueItem::on_updateButton_clicked()
{
    data->app->TryRunUpdate();
    
}
