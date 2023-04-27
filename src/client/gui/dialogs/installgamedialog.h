#ifndef INSTALLGAMEDIALOG_H
#define INSTALLGAMEDIALOG_H

#include <QWizard>
#include "../../interop/app.h"

namespace Ui {
class InstallGameDialog;
}

class InstallGameDialog : public QWizard
{
    Q_OBJECT

public:
    explicit InstallGameDialog(QWidget *parent, App *app);
    ~InstallGameDialog();
    void PopulateLibraryFolders();

private slots:
    void on_installLocationBox_activated(int index);
    void on_InstallGameDialog_currentIdChanged(int id);
    void installFailed(EAppUpdateError err);

private:
    App *app;
    Ui::InstallGameDialog *ui;
};

#endif // INSTALLGAMEDIALOG_H
