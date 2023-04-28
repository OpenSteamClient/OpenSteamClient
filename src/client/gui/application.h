#include <QObject>
#include <QCoreApplication>
#include <QThread>
#include <QSettings>
#include <string>
#include "windows/mainwindow.h"
#include "dialogs/startupprogressdialog.h"
#include "../../common/commandline.h"
#include "../threading/threadcontroller.h"
#include "windows/loginwindow.h"
#include "../login/loginthread.h"

#pragma once

// Our "main" gui class
class Application : public QObject
{
    Q_OBJECT
private:
    static Application *instance;
    Application();
    // Called from quitApp
    void Shutdown(bool bRestoreValveSteam = false);
    void loginFailed(SteamServerConnectFailure_t);
    void loginSucceeded(SteamServersConnected_t);
    bool hasLogonCompleted = false;

public:
    QCoreApplication *QApp;
    MainWindow *mainWindow;
    CommandLine *commandLine;
    QSettings *settings;
    LoginWindow *loginWindow;
    StartupProgressDialog *progDialog;
    AppManager *appManager;

    uint64_t currentUserSteamID = 0;
    static Application *GetApplication();
    int StartApplication();
    bool IsGUIBoot();
    void InitApplication();
    ~Application();
private slots:
    void postLogonState(PostLogonState_t state);
public slots:
    void quitApp();
    void quitAppAndRestoreValveSteam();
signals:
    void quitRequested();
};