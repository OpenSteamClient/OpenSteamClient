#include "loginwindow.h"
#include "ui_loginwindow.h"
#include "../../globals.h"
#include "../../threading/threadcontroller.h"
#include "../../interop/callbackthread.h"
#include "../../ext/steamclient.h"
#include "../../ext/steamclient.h"
#include "../application.h"
#include "../../interop/errmsgutils.h"
#include <QMessageBox>
#include <QPushButton>
#include <QPainter>
#include <QLayout>
#include <qrencode.h>

LoginWindow::LoginWindow(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::LoginWindow)
{
    ui->setupUi(this);

    connect(this->ui->loginButton, &QPushButton::clicked, this, &LoginWindow::startLogin);

    // This widget is for styling purposes
    ui->qrParentWidget->setStyleSheet("background-color: white; border: 0px; border-radius: 16px;");

    QGridLayout *l = new QGridLayout(ui->qrParentWidget); 
    l->setContentsMargins(15, 15, 15, 15);

    qrWidget = new QRWidget();
    qrWidget->setObjectName("qrWidget");
    l->addWidget(qrWidget, 0, 0, 1, 1);

    // Hide all QR parts by default
    qrWidget->hide();
    ui->qrErrorLabel->hide();
    ui->regenerateQRButton->hide();
    ui->regenerateQRButton->setEnabled(false);

    // Connect the loginThread
    connect(Global_ThreadController->loginThread, &LoginThread::OnLogonFailed, this, &LoginWindow::loginFailed);
    connect(Global_ThreadController->loginThread, &LoginThread::OnLogonFinished, this, &LoginWindow::loginSucceeded);
    connect(Global_ThreadController->loginThread, &LoginThread::OnNeedsSecondFactor, this, &LoginWindow::secondFactorNeeded);
    connect(Global_ThreadController->loginThread, &LoginThread::QRCodeReady, this, &LoginWindow::qrCodeReady);
    connect(this, &LoginWindow::startLogonWithCredentials, Global_ThreadController->loginThread, &LoginThread::StartLogonWithCredentials, Qt::ConnectionType::QueuedConnection);

    LoadKnownUsers();

    // Hide remembered credential things
    ui->removeCachedButton->setVisible(false);
    ui->accountRemembered->setVisible(false);

}

void LoginWindow::showEvent( QShowEvent* event ) {
    QWidget::showEvent( event );
    
    // Start generating QR codes
    QMetaObject::invokeMethod(Global_ThreadController->loginThread, "StartGeneratingQRCodes", Qt::ConnectionType::QueuedConnection);
} 

void LoginWindow::setUsername(std::string username) {
    ui->usernameField->setCurrentText(QString::fromStdString(username));
}

LoginWindow::~LoginWindow()
{
    if (twofactordialog) {
        delete twofactordialog;
        twofactordialog = nullptr;
    }
    
    delete ui;
}

void LoginWindow::secondFactorNeeded() {
    twofactordialog = new TwoFactorDialog(this, Global_ThreadController->loginThread->allowedConfirmations);

    // Connect the twofactordialog
    // We don't explicitly re-allow input with this, LoginThread will allow it after fail
    connect(twofactordialog, &TwoFactorDialog::cancelRequested, Global_ThreadController->loginThread, &LoginThread::CancelLogin);

    twofactordialog->show();
}

void LoginWindow::qrCodeReady(std::string url) {
    qrWidget->setQRData(QString::fromStdString(url));
    qrWidget->show();
    qrWidget->repaint();
}

void LoginWindow::startLogin()
{
    std::string username = ui->usernameField->currentText().toStdString();
    std::string password = ui->passwordField->text().toStdString();
    bool rememberPassword = ui->rememberPasswordBox->isChecked();

    isLoginInProgress = true;
    UpdateUIState();

    emit startLogonWithCredentials(username, password, rememberPassword);
}
void LoginWindow::UpdateUIState() {
    ui->loginButton->setDisabled(isLoginInProgress);
    ui->usernameField->setDisabled(isLoginInProgress);
    ui->passwordField->setDisabled(isLoginInProgress);
    ui->rememberPasswordBox->setDisabled(isLoginInProgress);
}

void LoginWindow::on_regenerateQRButton_clicked() 
{
    ui->qrParentWidget->setStyleSheet("background-color: white; border: 0px; border-radius: 16px;");
    ui->qrErrorLabel->hide();
    ui->regenerateQRButton->hide();
    ui->regenerateQRButton->setEnabled(false);
    QMetaObject::invokeMethod(Global_ThreadController->loginThread, "StartGeneratingQRCodes", Qt::ConnectionType::QueuedConnection);
}

void LoginWindow::loginFailed(std::string msg, EResult eResult) {
    std::cout << "[LoginWindow] Failed to log on " << msg << std::endl;
    isLoginInProgress = false;
    UpdateUIState();

    // Can this be emitted from credential logins?
    if (eResult == k_EResultFileNotFound) {
        ui->qrParentWidget->setStyleSheet("");
        qrWidget->hide();
        ui->qrErrorLabel->show();
        ui->regenerateQRButton->show();
        ui->regenerateQRButton->setEnabled(true);
    }

    //TODO: check if this is a QR login
    if (eResult == k_EResultExpired) {
        ui->qrParentWidget->setStyleSheet("");
        qrWidget->hide();
        ui->qrErrorLabel->show();
        ui->regenerateQRButton->show();
        ui->regenerateQRButton->setEnabled(true);
        return;
    }

    Application::GetApplication()->progDialog->hide();

    QMessageBox msgBox;
    msgBox.setText(QString("Failed to log on %1").arg(ui->usernameField->currentText()));
    msgBox.setInformativeText(QString("Error: %1, eResult: %2").arg(QString::fromStdString(msg), QString::fromStdString(ErrMsgUtils::GetErrorMessageFromEResult(eResult))));
    msgBox.exec();
}

void LoginWindow::logoutAndShowWindow(bool bForgetCredentials) {
    size_t bufSize = 256;
    char *username = new char[bufSize];
    Global_SteamClientMgr->ClientUser->GetAccountName(username, bufSize);

    Application::GetApplication()->hasLogonCompleted = false;
    Global_SteamClientMgr->ClientUser->LogOff();
    while (Global_SteamClientMgr->ClientUser->BConnected()) {
        std::this_thread::sleep_for(std::chrono::milliseconds(50));
    }
    if (bForgetCredentials) {
        Global_ThreadController->loginThread->RemoveCachedCredentials(std::string(username));
    }

    delete[] username;

    LoadKnownUsers();

    show();
}

void LoginWindow::loginSucceeded() {
    this->hide();
    if (twofactordialog) {
        twofactordialog->hide();
        delete twofactordialog;
        twofactordialog = nullptr;
    }
    isLoginInProgress = false;
    UpdateUIState();
    Application::GetApplication()->progDialog->show();
    Application::GetApplication()->progDialog->UpdateProgressText("Waiting for steamclient...");
}

void LoginWindow::on_usernameField_currentTextChanged(const QString &arg1)
{
    bool isRemembered = Global_SteamClientMgr->ClientUser->BHasCachedCredentials(arg1.toStdString().c_str());

    ui->removeCachedButton->setVisible(isRemembered);
    ui->accountRemembered->setVisible(isRemembered);

    ui->passwordField->setVisible(!isRemembered);
    ui->rememberPasswordBox->setVisible(!isRemembered);
    ui->passwordLabel->setVisible(!isRemembered);
}

void LoginWindow::on_removeCachedButton_clicked()
{
    std::string username = ui->usernameField->currentText().toStdString();
    ui->usernameField->removeItem(ui->usernameField->currentIndex());

    Global_ThreadController->loginThread->RemoveCachedCredentials(username);

    LoadKnownUsers();

    on_usernameField_currentTextChanged(QString::fromStdString(username));
}

void LoginWindow::LoadKnownUsers() {
    ui->usernameField->clear();
    for (const auto element : Global_ThreadController->loginThread->GetRememberedUsers())
    {
        ui->usernameField->addItem(element);
    }
}