#include "../../globals.h"
#include "../../threading/threadcontroller.h"

#include "twofactordialog.h"
#include "ui_twofactordialog.h"

TwoFactorDialog::TwoFactorDialog(QWidget *parent) :
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
}

TwoFactorDialog::~TwoFactorDialog()
{
    delete ui;
}

// Clicking this button means the user has entered a Steam Guard code and wants to send it
//TODO: we should communicate it more clearly
void TwoFactorDialog::acceptButton_clicked() {
    DEBUG_MSG << "clicked" << std::endl;

    if (ui->steamGuardCodeField->text().isEmpty())
    {
        //TODO: convey this to the user
        return;
    }

    Global_ThreadController->loginThread->AddSteamGuardCode(ui->steamGuardCodeField->text().toStdString());
    emit guardCodeSent();
}

// User wants to cancel the login
void TwoFactorDialog::cancelButton_clicked() {
    hide();
    emit cancelRequested();
}

