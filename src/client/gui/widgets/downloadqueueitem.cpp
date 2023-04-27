#include "downloadqueueitem.h"
#include "ui_downloadqueueitem.h"
#include <QString>

DownloadQueueItem::DownloadQueueItem(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::DownloadQueueItem)
{
    ui->setupUi(this);
}

void DownloadQueueItem::SetItemData(DownloadQueueItemData data) {
    this->data = data;
    ui->gameName->setText(QString::fromStdString(data.name));
    ui->downloaded->setText(QString::fromStdString(std::to_string(data.initialUpdateInfo.m_unBytesDownloaded)));
    UpdateUpdateInfo(data.initialUpdateInfo);
}

void DownloadQueueItem::UpdateUpdateInfo(AppUpdateInfo_s updateInfo)  
{
    std::string totalDownloaded = std::to_string((float)(updateInfo.m_unBytesDownloaded / (float)1024 / (float)1024 / (float)1024)).substr(0, 4);
    std::string totalToDownload = std::to_string((float)(updateInfo.m_unBytesToDownload / (float)1024 / (float)1024 / (float)1024)).substr(0, 4);
    std::string totalProcessed = std::to_string((float)(updateInfo.m_unBytesProcessed / (float)1024 / (float)1024 / (float)1024)).substr(0, 4);
    std::string totalToProcess = std::to_string((float)(updateInfo.m_unBytesToProcess / (float)1024 / (float)1024 / (float)1024)).substr(0, 4);
    QString totalUnit = "GB";

    ui->downloaded->setText(QString("%1 %2 / %3 %2").arg(QString::fromStdString(totalDownloaded), totalUnit, QString::fromStdString(totalToDownload)));
    ui->installed->setText(QString("%1 %2 / %1 %2").arg(QString::fromStdString(totalProcessed), totalUnit, QString::fromStdString(totalToProcess)));

    double divideResult = (double)updateInfo.m_unBytesDownloaded / (double)updateInfo.m_unBytesToDownload;
    int percentDownloaded = static_cast<int>(divideResult * 100);
    ui->progressBar->setValue(percentDownloaded);
}

DownloadQueueItem::~DownloadQueueItem()
{
    delete ui;
}

void DownloadQueueItem::on_updateButton_clicked()
{
    emit RequestUpdate(data.appid);
}
