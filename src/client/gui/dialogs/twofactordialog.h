#ifndef TWOFACTORDIALOG_H
#define TWOFACTORDIALOG_H

#include <QDialog>
#include <steammessages_auth.steamclient.pb.h>

namespace Ui {
class TwoFactorDialog;
}

//TODO: logging in with a code is not techincally always possible, we should hide the option when it isn't
class TwoFactorDialog : public QDialog
{
    Q_OBJECT

public:
    explicit TwoFactorDialog(QWidget *parent, std::vector<CAuthentication_AllowedConfirmation> allowedConfirmations);
    ~TwoFactorDialog(); 
signals:
    void cancelRequested();
    void guardCodeSent();


public slots:
    void acceptButton_clicked();
    void cancelButton_clicked();

private slots:
    void on_steamGuardCodeField_textChanged(const QString &arg1);

private:
    Ui::TwoFactorDialog *ui;
    std::vector<CAuthentication_AllowedConfirmation> allowedConfirmations;
    EAuthSessionGuardType codeType = k_EAuthSessionGuardType_None;
};

#endif // TWOFACTORDIALOG_H
