#include "downloaddebuggui.h"
#include "ui_downloaddebuggui.h"
#include "../../interop/downloads.h"
#include "../../ext/steamclient.h"
#include "../application.h"
#include "../../interop/appmanager.h"

#include <opensteamworks/IClientAppManager.h>

extern void LogAppUpdateInfo(AppUpdateInfo_s);

DownloadDebugGui::DownloadDebugGui(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::DownloadDebugGui)
{
    ui->setupUi(this);
    this->mgr = Application::GetApplication()->appManager->downloadManager;
    ReloadDownloads();
}

DownloadDebugGui::~DownloadDebugGui()
{
    delete ui;
}

void DownloadDebugGui::on_pushButton_clicked()
{
    mgr->ReloadDownloadInfo();
    ReloadDownloads();
}

void DownloadDebugGui::ReloadDownloads() {

    ui->queue->clear();
    ui->unscheduled->clear();
    ui->scheduled->clear();
    
    for (auto &&i : this->mgr->queue)
    {
        auto item = new QListWidgetItem();
        item->setText(QString::fromStdString(i.second->app->name));
        item->setData(Qt::UserRole, QVariant::fromValue<DownloadItem *>(i.second));
        ui->queue->addItem(item);
    }

    for (auto &&i : this->mgr->unscheduled)
    {
        auto item = new QListWidgetItem();
        item->setText(QString::fromStdString(i.second->app->name));
        item->setData(Qt::UserRole, QVariant::fromValue<DownloadItem *>(i.second));
        ui->unscheduled->addItem(item);
    }

    for (auto &&i : this->mgr->scheduled)
    {
        auto item = new QListWidgetItem();
        item->setText(QString::fromStdString(i.second->app->name + " | " + std::to_string(i.first)));
        item->setData(Qt::UserRole, QVariant::fromValue<DownloadItem *>(i.second));
        ui->scheduled->addItem(item);
    }
}

void DownloadDebugGui::on_pushButton_2_clicked()
{
    // Print download info
    AppUpdateInfo_s updateInfo;
    Global_SteamClientMgr->ClientAppManager->GetUpdateInfo(ui->lineEdit->text().toUInt(), &updateInfo);
    LogAppUpdateInfo(updateInfo);

}

void DownloadDebugGui::on_moveToQueue_clicked()
{

}


void DownloadDebugGui::on_moveToUnscheduled_clicked()
{

}


void DownloadDebugGui::on_moveToScheduled_clicked()
{

}


void DownloadDebugGui::on_upBtn_clicked()
{

}


void DownloadDebugGui::on_downBtn_clicked()
{

}

