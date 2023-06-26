#ifndef FILTERSPOPUP_H
#define FILTERSPOPUP_H

#include <QWidget>

namespace Ui {
class FiltersPopup;
}

class AppModel;

class FiltersPopup : public QWidget
{
    Q_OBJECT

public:
    explicit FiltersPopup(AppModel *model, QWidget *parent = nullptr);
    ~FiltersPopup();
private:
    void updateSelectedTypes();
    
private slots:
    void on_appState_notInstalled_stateChanged(int arg1);

    void on_appState_installed_stateChanged(int arg1);

    void on_appState_updateRequired_stateChanged(int arg1);

    void on_appState_updating_stateChanged(int arg1);

    void on_appState_running_stateChanged(int arg1);

    void on_appType_game_stateChanged(int arg1);

    void on_appType_application_stateChanged(int arg1);

    void on_appType_music_stateChanged(int arg1);

private:
    Ui::FiltersPopup *ui;
    AppModel *model;
};

#endif // FILTERSPOPUP_H
