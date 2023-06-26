#include "startupprogressdialog.h"
#include "ui_startupprogressdialog.h"

StartupProgressDialog::StartupProgressDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::StartupProgressDialog)
{
    ui->setupUi(this);

    // The progress bar should be initially invisible
    ui->progressBar->setVisible(false);

    // Disable the minimize, maximize, and close buttons 
    setWindowFlags(Qt::Window | Qt::WindowTitleHint | Qt::CustomizeWindowHint);
}

StartupProgressDialog::~StartupProgressDialog()
{
    delete ui;
}

void StartupProgressDialog::UpdateProgressText(QString newText) {
    ui->currentActionLabel->setText(newText);
}
void StartupProgressDialog::UpdateProgressBar(int prog, bool enabled) {
    ui->progressBar->setVisible(enabled);
    ui->progressBar->setValue(prog);
}
void StartupProgressDialog::SetProgressBarMax(int maxProg) {
    ui->progressBar->setMaximum(maxProg);
}
