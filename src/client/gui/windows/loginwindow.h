#ifndef LOGINWINDOW_H
#define LOGINWINDOW_H

#include <QDialog>
#include "../../interop/includesteamworks.h"
#include "../widgets/qrwidget.h"
#include "../dialogs/twofactordialog.h"

namespace Ui {
class LoginWindow;
}

class LoginWindow : public QDialog
{
    Q_OBJECT

public:
    void showEvent( QShowEvent* event );
    explicit LoginWindow(QWidget *parent = nullptr);
    ~LoginWindow();
private:
    bool isLoginInProgress = false;
    void UpdateUIState();
    QRWidget *qrWidget;
    TwoFactorDialog *twofactordialog;

signals:
    void startLogonWithCredentials(std::string username, std::string password, bool rememberPassword);

public slots:
    void on_regenerateQRButton_clicked();
    void setUsername(std::string username);
    void logoutAndShowWindow();

private slots:
    void startLogin();
    void loginFailed(std::string msg, EResult eResult);
    void loginSucceeded();
    void secondFactorNeeded();
    void qrCodeReady(std::string url);

private:
    Ui::LoginWindow *ui;
};

#endif // LOGINWINDOW_H
