#pragma once

#include <QWidget>
#include <QUrl>
#include "../../interop/includesteamworks.h"

// Forward declare
class AbstractDynamicWebViewInterface;

namespace Ui {
class DynamicWebViewWidget;
}

// A dynamic webview, loaded only when needed.
class DynamicWebViewWidget : public QWidget
{
    Q_OBJECT

public:
    // Initializes the dynamic web view. 
    // Note: No libraries are loaded at this stage
    explicit DynamicWebViewWidget(QWidget *parent = nullptr);
    ~DynamicWebViewWidget();

    // Loads the webview's libraries. 
    // Doesn't load any web page.
    // Calling this function will increase RAM use.
    void LoadWebView();

    // Unloads the webview's libraries and shows webViewStatus.
    // Calling this function will decrease RAM use.
    void UnloadWebView();

    // Loads a url. If it's found to be an official Steam url, it will add the necessary login cookies.
    // Also hides the webViewStatus.
    void LoadURL(QUrl url);

public:
    bool isWebViewLoaded = false;
    AbstractDynamicWebViewInterface *interface = nullptr;
    QWidget *webEngineView = nullptr;


private slots:
    void on_backButton_clicked();

    void on_nextButton_clicked();

    void on_refreshButton_clicked();

    void OnUrlChanged(QUrl url);

    void OnWebAuthRequestCallback(WebAuthRequestCallback_t data);

private:
    Ui::DynamicWebViewWidget *ui;
};
