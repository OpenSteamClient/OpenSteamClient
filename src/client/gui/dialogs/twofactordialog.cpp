#include "../../globals.h"
#include "../../threading/threadcontroller.h"

#include "twofactordialog.h"
#include "ui_twofactordialog.h"

TwoFactorDialog::TwoFactorDialog(QWidget *parent, std::vector<CAuthentication_AllowedConfirmation> allowedConfirmations) :
    QDialog(parent),
    ui(new Ui::TwoFactorDialog)
{
    ui->setupUi(this);

    connect(this->ui->acceptButton, &QPushButton::clicked, this, &TwoFactorDialog::acceptButton_clicked);
    connect(this->ui->cancelButton, &QPushButton::clicked, this, &TwoFactorDialog::cancelButton_clicked);

    // Steam guard code in all uppercase
    QFont f = font();
    f.setCapitalization(QFont::AllUppercase);
    ui->steamGuardCodeField->setFont(f);
    this->allowedConfirmations = allowedConfirmations;
    std::vector<QString> allowedConfirmationsText;
    for (auto &&confirmation : allowedConfirmations)
    {
        switch (confirmation.confirmation_type())
        {
        case k_EAuthSessionGuardType_DeviceCode:
            codeType = k_EAuthSessionGuardType_DeviceCode;
            allowedConfirmationsText.push_back(QString("- Code from Steam Mobile app"));
            break;

        case k_EAuthSessionGuardType_DeviceConfirmation:
            allowedConfirmationsText.push_back(QString("- Steam Mobile confirmation"));
            break;

        case k_EAuthSessionGuardType_EmailCode:
            codeType = k_EAuthSessionGuardType_EmailCode;
            allowedConfirmationsText.push_back(QString("- Code from your email at %1").arg(QString::fromStdString(confirmation.associated_message())));
            break;

        case k_EAuthSessionGuardType_EmailConfirmation:
            allowedConfirmationsText.push_back(QString("- Confirmation from your email at %1").arg(QString::fromStdString(confirmation.associated_message())));
            break;

        default:
            break;
        }   
    }
    QString allowedConfirmationsConcatText;
    for (auto &&i : allowedConfirmationsText)
    {
        allowedConfirmationsConcatText += i + "\n";
    }
    ui->allowedConfirmationsLabel->setText(allowedConfirmationsConcatText);
}

TwoFactorDialog::~TwoFactorDialog()
{
    delete ui;
}

// Clicking this button means the user has entered a Steam Guard code and wants to send it
void TwoFactorDialog::acceptButton_clicked() {
    Global_ThreadController->loginThread->AddSteamGuardCode(ui->steamGuardCodeField->text().toStdString(), codeType);
    emit guardCodeSent();
}

// User wants to cancel the login
void TwoFactorDialog::cancelButton_clicked() {
    hide();
    emit cancelRequested();
}


void TwoFactorDialog::on_steamGuardCodeField_textChanged(const QString &arg1)
{
    ui->acceptButton->setEnabled(!arg1.isEmpty());
}
