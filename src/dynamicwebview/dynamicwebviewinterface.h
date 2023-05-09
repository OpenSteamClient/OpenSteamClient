#pragma once

#include <QWidget>
#include <QWebEngineView>
#include <QWebEngineProfile>
#include <QWebEngineCookieStore>

class AbstractDynamicWebViewInterface {
public:
    virtual QWebEngineView *GetWebViewWidget(QWidget *parent) = 0;
    virtual void DeleteWebEngine() = 0;
    virtual void load(QUrl url) = 0;
    virtual void setCookie(QNetworkCookie cookie) = 0;
    virtual void setUserAgent(std::string useragent) = 0;
    // Basically the same as QObject::connect, except it only works for the webengine and with strings
    // Make sure the signal and slot exist 
    virtual void connect(std::string signalName, QObject *classRef, std::string slotName) = 0;
    virtual void back() = 0;
    virtual void forward() = 0;
    virtual void reload() = 0;
    virtual bool canGoBack() = 0;
    virtual bool canGoForward() = 0;
    virtual QUrl currentURL() = 0;
};

class QQuickWidget;
class QQmlEngine;

class DynamicWebViewInterface : AbstractDynamicWebViewInterface
{
private:
    QWebEngineView *webEngineView = nullptr;
    QQuickWidget *quickWidget = nullptr;
    QQmlEngine *qmlEngine = nullptr;

public:
    // The parent is required, since we're being loaded externally
    QWebEngineView *GetWebViewWidget(QWidget *parent);
    // TODO: doesn't work
    void DeleteWebEngine();
    void load(QUrl url);
    void setCookie(QNetworkCookie cookie);
    void setUserAgent(std::string useragent);
    void connect(std::string signalName, QObject *classRef, std::string slotName);
    void back();
    void forward();
    void reload();
    bool canGoBack();
    bool canGoForward();
    QUrl currentURL();
    DynamicWebViewInterface(/* args */);
    ~DynamicWebViewInterface();
};