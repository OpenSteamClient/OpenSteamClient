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
    void LoadKnownUsers();
    QRWidget *qrWidget = nullptr;
    TwoFactorDialog *twofactordialog = nullptr;

signals:
    void startLogonWithCredentials(std::string username, std::string password, bool rememberPassword);

public slots:
    void on_regenerateQRButton_clicked();
    void setUsername(std::string username);
    void logoutAndShowWindow(bool bForgetCredentials = false);

private slots:
    void startLogin();
    void loginFailed(std::string msg, EResult eResult);
    void loginSucceeded();
    void secondFactorNeeded();
    void qrCodeReady(std::string url);

    void on_usernameField_currentTextChanged(const QString &arg1);

    void on_removeCachedButton_clicked();

private:
    Ui::LoginWindow *ui;
};

#endif // LOGINWINDOW_H
