#include "aboutdialog.h"
#include "ui_aboutdialog.h"
#include <gitinfo.h>
#include "../../ext/steamclient.h"
#include <opensteamworks/IClientUtils.h>

AboutDialog::AboutDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AboutDialog)
{
    ui->setupUi(this);

    // Would like to log this info too, but it doesn't work.
    // A call to ClientUtils->GetBuildID() doesn't work and returns 0. Why?


    ui->steamclientVersion->setText(ui->steamclientVersion->text().arg(CLIENTENGINE_VERSION));
    ui->qtVersion->setText(ui->qtVersion->text().arg(QT_VERSION_STR));
    ui->buildCommit->setText(ui->buildCommit->text().arg(GIT_COMMIT_HASH, GIT_BRANCH_NAME));
}

AboutDialog::~AboutDialog()
{
    delete ui;
}
