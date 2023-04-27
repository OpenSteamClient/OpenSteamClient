#include "appmanager.h"
#include "../threading/threadcontroller.h"
#include "callbackthread.h"
#include <mutex>
#include "../utils/binarykv.h"
#include <QImageReader>
#include <QBuffer>
#include <filesystem>
namespace fs = std::filesystem;

AppManager::AppManager() {
    connect(Global_ThreadController->callbackThread, &CallbackThread::AppLicensesChanged, this, &AppManager::cbLicensesChanged);
    this->manager = new QNetworkAccessManager(this);

}

void AppManager::cbLicensesChanged(AppLicensesChanged_t changeInfo) {
    std::lock_guard<std::mutex> guard(licensesMutex);
    for (size_t i = 0; i < changeInfo.m_unAppsUpdated; i++)
    {
        this->licenses.push_back(changeInfo.m_rgAppsUpdated[i]);
    }
}

void AppManager::LoadApps() {
    char *type = new char[128];
    char *name = new char[1024];
    char *hero_url = new char[1024];
    char *logo_url = new char[1024];

    // TODO: use GetAppStateInfo to get info
    for (auto &&i : this->licenses)
    {
        if (Global_SteamClientMgr->ClientApps->GetAppData(i, "common/type", type, 128) == 0)
        {
            DEBUG_MSG << "Skipping " << i << " no type returned" << std::endl;
            continue;
        }

        Global_SteamClientMgr->ClientApps->GetAppData(i, "common/name", name, 1024);

        App *app = new App(i);
        app->name = std::string(name);
        app->TypeFromString(type);
        app->UpdateAppState();

        size_t bufLen = 1000000;
        std::vector<uint8_t> buf(bufLen);
        std::vector<std::string> categories;

        // Support at most 512 categories
        // Why? There's no way to get an array from the config store. 
        for (size_t i2 = 0; i2 < 512; i2++)
        {
            std::string keyName = std::string("Software/Valve/Steam/Apps/").append(std::to_string(i)).append("/Tags/").append(std::to_string(i2));
            const char* returned = Global_SteamClientMgr->ClientConfigStore->GetString(k_EConfigStoreUserRoaming, keyName.c_str(), "");
            if (std::string(returned) == "") {
                DEBUG_MSG << "key not found..." << std::endl;
                break;
            }
            else
            {
                app->inCategories.push_back(std::string(returned));
            }
        }

        //ui->gamesListWidget->addItem(new QListWidgetGameItem(i, name, type));

        //TODO: is this logic correct?
        if (app->updateInfo.m_unBytesToDownload > 0) {
            // If an update is available, add it to the map (not the queue)
            //appsWithUpdates.insert(app);
        }
        this->apps.insert({i, app});
    }
    StartLoadingLibraryAssets();
}

void AppManager::StartLoadingLibraryAssets() {

    // Ensure the library cache exists
    if (!fs::exists("appcache/librarycache")) {
        fs::create_directories("appcache/librarycache");
    }


    for (auto &&i : this->apps)
    {
        RequestLibraryAsset(i.second, ArtworkType::Icon);
    }
}

void AppManager::RequestLibraryAsset(App* app, ArtworkType type) 
{
    char *assetHashCStr = new char[1024];
    QString downloadUrl = QString("https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/%1/").arg(QString::fromStdString(std::to_string(app->appid)));
    QString assetHash;
    bool success = false;

    switch (type)
    {
    case ArtworkType::Icon:
    {   
        if (app->type == k_EAppTypeMusic) {
            // Music isn't supported yet
            break;
        }

        int32 returnedLength = Global_SteamClientMgr->ClientApps->GetAppData(app->appid, "common/clienticon", assetHashCStr, 1024);
        if (returnedLength == 0) {
            // If we didn't get a response, skip
            break;
        }

        if (std::string(assetHashCStr).empty()) {
            // If the hash is empty, skip
            break;
        }

        assetHash = QString::fromStdString(std::string(assetHashCStr));
        app->libraryAssets.iconHash = assetHash.toStdString();

        // appcache/librarycache/%1_%2.png
        std::string cachedFilename = std::string("appcache/librarycache/").append(std::to_string(app->appid)).append("_").append(assetHash.toStdString()).append(".png");
        if (fs::exists(cachedFilename))
        {
            // If the file exists, don't redownload
            app->libraryAssets.iconCachedFilename = cachedFilename;
            DEBUG_MSG << "this one is cached" << std::endl;
            break;
        }

        downloadUrl = downloadUrl.append(QString("%1.ico").arg(assetHash));
        
        success = true;
        break;
    }

    default:
        break;
    }

    delete[] assetHashCStr;

    if (success) {

        {
            std::lock_guard<std::mutex> guard(mutex_pendingHashToTypeMap);
            pendingHashToTypeMap.insert({assetHash, type});
        }
        

        QNetworkRequest request;
        request.setUrl(QUrl(downloadUrl));
        request.setRawHeader("User-Agent", "OpenSteamClient 1.0");

        QNetworkReply *reply;
        {
            std::lock_guard<std::mutex> guard(mutex_pendingReplyToAppMap);
            reply = manager->get(request);
            pendingReplyToAppMap.insert({reply, app});
        }

        connect(reply, &QNetworkReply::finished, this, &AppManager::replyReceived);
        connect(reply, &QNetworkReply::errorOccurred,
                this, &AppManager::replyErrored);
    }
}

void AppManager::replyReceived() {
    QNetworkReply *reply = qobject_cast<QNetworkReply*>(sender());

    // I haven't figured out why it is sometimes missing the reply, but nothing we can do in that case
    if (!pendingReplyToAppMap.contains(reply)) {
        return;
    }

    App *forApp = pendingReplyToAppMap.at(reply);

    if (reply->error() == QNetworkReply::NoError)
    {
        QString artworkHash = QString::fromStdString(forApp->libraryAssets.iconHash);
        DEBUG_MSG << "len is " << std::to_string(pendingHashToTypeMap.size()) << "hash is " << artworkHash.toStdString() << std::endl;
        if (!pendingHashToTypeMap.contains(artworkHash)) {
            DEBUG_MSG << "not contained :(" << std::endl;
            {
                std::lock_guard<std::mutex> guard(mutex_pendingReplyToAppMap);
                pendingReplyToAppMap.erase(reply);
            }

            return;
        }
        ArtworkType type = pendingHashToTypeMap.at(artworkHash);

        QByteArray iconAsBytes = reply->readAll();
        QBuffer device(&iconAsBytes);
        device.open(QIODevice::ReadOnly);

        QImageReader imageReader = QImageReader(&device);
        
        QImage bestImage;
        if (imageReader.imageCount() > 0)
        {
            // This algorithm determines the most ideal icon to use from the ico collection of icons
            for (size_t i = 0; i < imageReader.imageCount(); i++)
            {
                imageReader.jumpToImage(i);
                QImage currentImage = imageReader.read();
                bool isLastImage = imageReader.imageCount() == i;

                if (currentImage.size() == QSize(32, 32))
                {
                    DEBUG_MSG << "icon is 32x32" << std::endl;
                    bestImage = currentImage;
                    // This is the best size for an icon, more is overkill, less is blurry
                    break;
                }
                else if ( currentImage.size().width() > 32 && currentImage.size().height() > 32) 
                {
                    DEBUG_MSG << "icon is good enough " << currentImage.size().width() << "x" << currentImage.size().height() << std::endl;
                    if (!bestImage.isNull()) {
                        if (bestImage.size().width() > currentImage.size().width() && bestImage.size().height() > currentImage.size().width()) {
                            // Set the image (outside of these if's)
                        } else {
                            // Skip this one
                            continue;
                        }
                    }
                    bestImage = currentImage;
                }
                else
                {
                    // This is not the ideal image, skip and go to the next one, unless it's the last image
                    if (isLastImage) {
                        DEBUG_MSG << "not an ideal image, but we need one " << currentImage.size().width() << "x" << currentImage.size().height() << std::endl;
                        bestImage = currentImage;
                    }
                }
            }
        } else {
            DEBUG_MSG << "We have only one image" << std::endl;
            bestImage = imageReader.read();
        }

        QString imageFilename = QString("appcache/librarycache/%1_%2.png").arg(QString::fromStdString(std::to_string(forApp->appid)), artworkHash);
        bestImage.save(imageFilename);

        forApp->libraryAssets.iconCachedFilename = imageFilename.toStdString();

        forApp->SetLibraryAssetsAvailable();

        device.close();

        {
            std::lock_guard<std::mutex> guard(mutex_pendingHashToTypeMap);
            pendingHashToTypeMap.erase(artworkHash);
        }
    }

    {
        std::lock_guard<std::mutex> guard(mutex_pendingReplyToAppMap);
        pendingReplyToAppMap.erase(reply);
    }   
    PendingReplyToAppMapChange();
    
    reply->deleteLater();
}


// A few games and software titles have no icon or the url is invalid. We don't cache these results.
void AppManager::replyErrored(QNetworkReply::NetworkError err) {
    QNetworkReply *reply = qobject_cast<QNetworkReply*>(sender());
    DEBUG_MSG << "Error occurred while fetching resource " << reply->url().toString().toStdString() << std::endl;

    {
        std::lock_guard<std::mutex> guard(mutex_pendingReplyToAppMap);
        if (pendingReplyToAppMap.contains(reply)) {
            pendingReplyToAppMap.erase(reply);
        } else {
            DEBUG_MSG << "pendingReplyToAppMap doesn't contain " << reply->url().toString().toStdString() << std::endl;
        }
    }
    PendingReplyToAppMapChange();

    reply->deleteLater();
}

void AppManager::PendingReplyToAppMapChange() {
    if (pendingReplyToAppMap.size() == 0) {
        emit libraryAssetsLoaded();
    }
}