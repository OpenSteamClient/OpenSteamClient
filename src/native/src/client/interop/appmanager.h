#include <vector>
#include "includesteamworks.h"
#include "../globals.h"
#include <QObject>
#include "app.h"
#include <QNetworkAccessManager>
#include <QNetworkDiskCache>
#include <QNetworkRequest>
#include <QNetworkReply>

#pragma once

enum ArtworkType
{
    Icon,
    Logo,
    Hero
};

class DownloadManager;

class AppManager : public QObject
{
    Q_OBJECT

public:
    std::vector<CompatTool *> compatTools;
    std::map<AppId_t, App *> apps;
    std::vector<AppId_t> licenses;
    DownloadManager *downloadManager;
    AppManager();
    ~AppManager() {}


public slots:
    void cbLicensesChanged(AppLicensesChanged_t changeInfo);
    void LoadApps();
    void StartLoadingLibraryAssets();
    void RequestLibraryAsset(App *app, ArtworkType type);

signals:
    void valueChanged(int newValue);
    void libraryAssetsLoaded();
    
private slots:
    void replyReceived();
    void replyErrored(QNetworkReply::NetworkError);

private:
    void PendingReplyToAppMapChange();

private:
    int m_value;
    std::mutex licensesMutex;
    QNetworkAccessManager *manager;

    std::mutex mutex_pendingReplyToAppMap;
    std::map<QNetworkReply*, App*> pendingReplyToAppMap;

    std::mutex mutex_pendingHashToTypeMap;
    std::map<QString, ArtworkType> pendingHashToTypeMap;
};