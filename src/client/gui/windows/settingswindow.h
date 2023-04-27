#ifndef SETTINGSWINDOW_H
#define SETTINGSWINDOW_H

#include <QDialog>
#include "../../interop/app.h"
#include "../../ext/steamclient.h"

namespace Ui {
class SettingsWindow;
}

class SettingsWindow : public QDialog
{
    Q_OBJECT

public:
    explicit SettingsWindow(QWidget *parent, App *app);
    ~SettingsWindow();

private:
    void PopulateCompatTools();
    void PopulateBetas();
    void ReadLaunchOptions();

private slots:
    void on_enableProtonBox_stateChanged(int arg1);

    void on_compatToolBox_currentIndexChanged(int index);

    void on_testBetaKeyButton_clicked();

    void betaPasswordResponseReceived(CheckAppBetaPasswordResponse_t);

    void on_betasDropdown_activated(int index);

    void on_uninstallBtn_clicked();

    void on_launchOptionsField_editingFinished();

private:
    App *app;
    Ui::SettingsWindow *ui;
};

#endif // SETTINGSWINDOW_H
