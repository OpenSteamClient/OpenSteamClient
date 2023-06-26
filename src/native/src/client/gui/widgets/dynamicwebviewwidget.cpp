#include "dynamicwebviewwidget.h"
#include "ui_dynamicwebviewwidget.h"
#include "../../temporary_logging_solution.h"
#include "../application.h"
#include "../../../dynamicwebview/dynamicwebviewinterface.h"
#include <opensteamworks/IClientUser.h>
#include <QTime>


DynamicWebViewWidget::DynamicWebViewWidget(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::DynamicWebViewWidget)
{
    ui->setupUi(this);
}

DynamicWebViewWidget::~DynamicWebViewWidget()
{
    delete ui;
}

void DynamicWebViewWidget::on_backButton_clicked()
{
    interface->back();
}

void DynamicWebViewWidget::on_nextButton_clicked()
{
    interface->forward();
}


void DynamicWebViewWidget::on_refreshButton_clicked()
{
    interface->reload();
}

void DynamicWebViewWidget::LoadWebView()
{
    interface = Application::GetApplication()->dynamicWebViewLibraryMgr->GetInterface();
    if (interface == 0x0) {
        DEBUG_MSG << "[DynamicWebViewWidget] GetInterface failed" << std::endl;
        ui->webviewStatus->setText("GetInterface failed");
        return;
    }
    DEBUG_MSG << "[DynamicWebViewWidget] GetWebViewInterface succeeded" << std::endl;

    webEngineView = interface->GetWebViewWidget(ui->browserContainer);
    if (webEngineView == 0x0) {
        DEBUG_MSG << "[DynamicWebViewWidget] GetWebViewWidget returned null" << std::endl;
        ui->webviewStatus->setText("GetWebViewWidget returned null");
        return;
    }

    isWebViewLoaded = true;
    DEBUG_MSG << "[DynamicWebViewWidget] Added WebEngineView" << std::endl;
    
    // Hide status text and show the webview
    ui->webviewStatusContainer->setVisible(false);
    ui->browserContainerLayout->addWidget(webEngineView);

    interface->connect("urlChanged", this, "OnUrlChanged");
}

void DynamicWebViewWidget::OnUrlChanged(QUrl url) {
    ui->currentURLBox->setText(url.toString());
    ui->backButton->setEnabled(interface->canGoBack());
    ui->nextButton->setEnabled(interface->canGoForward());
}

void DynamicWebViewWidget::UnloadWebView()
{
    isWebViewLoaded = true;
    if (!isWebViewLoaded)
    {
        DEBUG_MSG << "[DynamicWebViewWidget] UnloadWebView called on unloaded webview!" << std::endl;
        return;
    }

    webEngineView->setUpdatesEnabled(false);
    ui->browserContainer->setUpdatesEnabled(false);
    ui->browserContainerLayout->removeWidget(webEngineView);

    webEngineView->setParent(nullptr);

    // Delete the webEngineView
    Application::GetApplication()->dynamicWebViewLibraryMgr->Unload(interface);

    // Re-enable the status text
    ui->webviewStatus->setText("Webview unloaded");
    //ui->webviewStatusContainer->setVisible(true);

    isWebViewLoaded = false;
    webEngineView = nullptr;
}

// This is probably as optimized as can be
// This list contains all official domains that we should treat differently
// There's probably more, but these will do fine for now
static constexpr const auto officialDomains = {
    "store.steampowered.com",
    "help.steampowered.com",
    "steamcommunity.com"
};

void DynamicWebViewWidget::OnWebAuthRequestCallback(WebAuthRequestCallback_t data) {
    if (data.m_bSuccessful) {
        DEBUG_MSG << "[DynamicWebViewWidget] Reloading webview, received webauth token" << std::endl;
        this->LoadURL(interface->currentURL());
    }
}

void DynamicWebViewWidget::LoadURL(QUrl url)
{
    if (!isWebViewLoaded) {
        DEBUG_MSG << "[DynamicWebViewWidget] LoadURL called on unloaded webview!" << std::endl;
        return;
    }
    std::string host = url.host().toStdString();
    // Check if the domain is an official Steam domain, if it is set credentials and useragent
    if (std::find(officialDomains.begin(), officialDomains.end(), host) != officialDomains.end())
    {
        DEBUG_MSG << "[DynamicWebViewWidget] LoadURL: Detected Official domain" << std::endl;

        const QDateTime sessionExpire = QDateTime(QDate(0, 0, 0), QTime(0, 0, 0, 0));
        std::string webAuthToken;
        size_t tokenSize = 1024;
        char *tokenCStr = (char *)malloc(tokenSize);
        if (Global_SteamClientMgr->ClientUser->GetCurrentWebAuthToken(tokenCStr, tokenSize))
        {
            DEBUG_MSG << "[DynamicWebViewWidget] Webauth token is cached" << std::endl;
            webAuthToken = std::string(tokenCStr);
            QNetworkCookie cookie;
            cookie.setName("steamLoginSecure");
            cookie.setValue(QByteArray::fromStdString(webAuthToken));
            cookie.setPath("/");
            cookie.setExpirationDate(sessionExpire);
            cookie.setDomain(url.host());
            cookie.setSecure(true);
            cookie.setHttpOnly(true);
            cookie.setSameSitePolicy(QNetworkCookie::SameSite::None);
            interface->setCookie(cookie);
        } else {
            DEBUG_MSG << "[DynamicWebViewWidget] Couldn't get cached webauth token, scheduling website reload for later..." << std::endl;
            Global_SteamClientMgr->ClientUser->RequestWebAuthToken();
            connect(Global_ThreadController->callbackThread, &CallbackThread::WebAuthRequestCallback, this, &DynamicWebViewWidget::OnWebAuthRequestCallback, Qt::ConnectionType::SingleShotConnection);
        }

        free((void *)tokenCStr);

        //TODO: once set, there is no way to clear the user agent, it should be cleared when loading a non-official website
        interface->setUserAgent("Valve Steam Client");

        //TODO: set clientsessionid


        //TODO: get client language and use that
        QNetworkCookie cookie2;
        cookie2.setName("Steam_Language");
        cookie2.setValue("english");
        cookie2.setPath("/");
        cookie2.setExpirationDate(sessionExpire);
        cookie2.setDomain(url.host());
        cookie2.setSecure(false);
        cookie2.setHttpOnly(false);
        cookie2.setSameSitePolicy(QNetworkCookie::SameSite::None);
        interface->setCookie(cookie2);
    }

    
    interface->load(url);
}
