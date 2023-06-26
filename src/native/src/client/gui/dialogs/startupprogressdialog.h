#ifndef STARTUPPROGRESSDIALOG_H
#define STARTUPPROGRESSDIALOG_H

#include <QDialog>

namespace Ui {
class StartupProgressDialog;
}

class StartupProgressDialog : public QDialog
{
    Q_OBJECT

public:
    explicit StartupProgressDialog(QWidget *parent = nullptr);
    ~StartupProgressDialog();

private:
    Ui::StartupProgressDialog *ui;

public slots:
    void UpdateProgressText(QString newText);
    void SetProgressBarMax(int maxProg);
    void UpdateProgressBar(int prog, bool enabled);
};

#endif // StartupProgressDialog_H
