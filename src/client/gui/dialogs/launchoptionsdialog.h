
#ifndef LAUNCHOPTIONSDIALOG_H
#define LAUNCHOPTIONSDIALOG_H

#include "../../interop/launchoption.h"
#include <QDialog>
#include <QListWidget>

namespace Ui {
class LaunchOptionsDialog;
}

class LaunchOptionsDialog : public QDialog
{
    Q_OBJECT

public:
    explicit LaunchOptionsDialog(QWidget *parent, std::vector<LaunchOption> opts);
    ~LaunchOptionsDialog();

private:
    bool selectedAnItem = false;
    Ui::LaunchOptionsDialog *ui;
    std::vector<LaunchOption> opts;

private slots:
    void launchOptionsList_selectionChanged(QListWidgetItem* current, QListWidgetItem *previous);
    void launchButton_clicked();
    void cancelButton_clicked();

signals:
    void OnOptionSelected(LaunchOption opt);
    void OnCancelled();
};

#endif // LAUNCHOPTIONSDIALOG_H
