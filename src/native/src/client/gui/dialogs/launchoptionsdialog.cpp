#include "launchoptionsdialog.h"
#include "ui_launchoptionsdialog.h"
#include "qlistwidgetlaunchoptionitem.h"

LaunchOptionsDialog::LaunchOptionsDialog(QWidget *parent, std::vector<LaunchOption> opts) :
    QDialog(parent),
    ui(new Ui::LaunchOptionsDialog)
{
    ui->setupUi(this);
    for (auto &&i : opts)
    {
        ui->launchOptionsList->addItem(new QListWidgetLaunchOptionItem(i));   
    }

    connect(ui->launchOptionsList, &QListWidget::currentItemChanged, this, &LaunchOptionsDialog::launchOptionsList_selectionChanged);
    connect(ui->launchButton, &QPushButton::clicked, this, &LaunchOptionsDialog::launchButton_clicked);
    connect(ui->cancelButton, &QPushButton::clicked, this, &LaunchOptionsDialog::cancelButton_clicked);

    this->setAttribute(Qt::WA_DeleteOnClose);
}

void LaunchOptionsDialog::launchOptionsList_selectionChanged(QListWidgetItem* current, QListWidgetItem *previous) {
    ui->launchButton->setEnabled(true);
}

void LaunchOptionsDialog::launchButton_clicked() {
    selectedAnItem = true;
    emit OnOptionSelected((*(QListWidgetLaunchOptionItem *)ui->launchOptionsList->currentItem()).opt);
    this->close();
}

void LaunchOptionsDialog::cancelButton_clicked() {
    emit OnCancelled();
    this->close();
}

LaunchOptionsDialog::~LaunchOptionsDialog()
{
    if (!selectedAnItem) {
        emit OnCancelled();
    }
    
    delete ui;
}
