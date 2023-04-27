#ifndef TWOFACTORDIALOG_H
#define TWOFACTORDIALOG_H

#include <QDialog>

namespace Ui {
class TwoFactorDialog;
}

//TODO: logging in with a code is not techincally always possible, we should hide the option when it isn't
class TwoFactorDialog : public QDialog
{
    Q_OBJECT

public:
    explicit TwoFactorDialog(QWidget *parent = nullptr);
    ~TwoFactorDialog(); 
signals:
    void cancelRequested();
    void guardCodeSent();


public slots:
    void acceptButton_clicked();
    void cancelButton_clicked();

private:
    Ui::TwoFactorDialog *ui;
};

#endif // TWOFACTORDIALOG_H
