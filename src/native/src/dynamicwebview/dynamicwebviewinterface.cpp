#include "dynamicwebviewinterface.h"
#include <QObject>
#include <QMetaMethod>
#include <iostream>
#include <QWebEngineHistory>
#include <QCoreApplication>
#include <QQmlEngine>
#include <QOpenGLContext>
#include <QQmlContext>
#include <QQuickWindow>
#include <QtWebEngineCore>
#include <QQuickWidget>
#include <QWebEngineView>
#include <QWebEngineCookieStore>

// This exists inside Qt in QOpenGLContext
extern QOpenGLContext *qt_gl_global_share_context();
extern void qt_gl_set_global_share_context(QOpenGLContext *context);

DynamicWebViewInterface::DynamicWebViewInterface(/* args */)
{
}

DynamicWebViewInterface::~DynamicWebViewInterface()
{
}

QWidget *DynamicWebViewInterface::GetWebViewWidget(QWidget *parent) {
    if (webEngineView != nullptr) {
        return webEngineView;
    }

    this->webEngineView = new QWebEngineView(this->quickWidget);
    return webEngineView;
}

void DynamicWebViewInterface::DeleteWebEngine() {
    webEngineView->setParent(nullptr);
    webEngineView->hide();
    
    webEngineView->page()->deleteLater();
    webEngineView->setPage(nullptr);

    webEngineView->close();

    delete webEngineView;

    webEngineView = nullptr;
}

void DynamicWebViewInterface::load(QUrl url) {
    webEngineView->setUrl(url);
}

void DynamicWebViewInterface::setCookie(QNetworkCookie cookie) {
    QWebEngineCookieStore *cookieStore = webEngineView->page()->profile()->cookieStore();
    cookieStore->setCookie(cookie);
}

void DynamicWebViewInterface::setUserAgent(std::string useragent) {
    QWebEngineProfile::defaultProfile()->setHttpUserAgent(QString::fromStdString(useragent));
}

QUrl DynamicWebViewInterface::currentURL() {
    return webEngineView->url();
}

void DynamicWebViewInterface::connect(std::string signalName, QObject *classRef, std::string slotName) {
    auto signal_metaobj = webEngineView->metaObject();
    QMetaMethod signal;
    for (size_t i = 0; i < signal_metaobj->methodCount(); i++)
    {
        QMetaMethod method = signal_metaobj->method(i);
        if (method.name().toStdString() == signalName) {
            signal = method;
            break;
        }
    }

    auto slot_metaobj = classRef->metaObject();
    QMetaMethod slot;
    for (size_t i = 0; i < slot_metaobj->methodCount(); i++)
    {
        QMetaMethod method = slot_metaobj->method(i);
        if (method.name().toStdString() == slotName) {
            slot = method;
            break;
        }
    }


    webEngineView->connect(webEngineView, signal, classRef, slot);
}

void DynamicWebViewInterface::back() {
    webEngineView->back();
}
void DynamicWebViewInterface::forward() {
    webEngineView->forward();
}
void DynamicWebViewInterface::reload() {
    webEngineView->reload();
}
bool DynamicWebViewInterface::canGoBack() {
    return webEngineView->history()->canGoBack();
}
bool DynamicWebViewInterface::canGoForward() {
    return webEngineView->history()->canGoForward();
}