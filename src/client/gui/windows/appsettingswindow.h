#ifndef APPSETTINGSWINDOW_H
#define APPSETTINGSWINDOW_H

#include <QWidget>
#include <QDialog>
#include <QListWidgetItem>

namespace Ui {
class AppSettingsWindow;
}

class AppSettingsWindow : public QDialog
{
    Q_OBJECT

public:
    explicit AppSettingsWindow(QWidget *parent = nullptr);
    ~AppSettingsWindow();

    void LoadLibraryFolders();
    void LoadCheckboxValues();
    void LoadCompatData();

private slots:
    void on_libraryFolders_currentItemChanged(QListWidgetItem *current, QListWidgetItem *previous);

    void on_addFolderButton_clicked();

    void on_deleteFolderButton_clicked();

    void on_enableCompatToolsCheck_stateChanged(int arg1);

    void on_allowDownloadsWhilePlayingCheck_stateChanged(int arg1);

    void on_autoLoginFriendsNetwork_box_stateChanged(int arg1);

    void on_defaultWOLCompatTool_currentIndexChanged(int index);

private:
    Ui::AppSettingsWindow *ui;
};

#endif // APPSETTINGSWINDOW_H
