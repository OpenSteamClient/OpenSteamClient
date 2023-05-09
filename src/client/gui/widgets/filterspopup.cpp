#include "filterspopup.h"
#include "ui_filterspopup.h"
#include "../gamelistview/appmodel.h"

FiltersPopup::FiltersPopup(AppModel *model, QWidget *parent) :
    QWidget(parent),
    ui(new Ui::FiltersPopup)
{
    ui->setupUi(this);
    this->model = model;
    AppState state = model->getStateFilter();
    ui->appState_installed->setChecked(state.FullyInstalled);
    ui->appState_notInstalled->setChecked(state.Uninstalled);
    ui->appState_updateRequired->setChecked(state.UpdateRequired);
    ui->appState_updating->setChecked(state.UpdateRunning);
    ui->appState_running->setChecked(state.AppRunning);

    for (auto &&i : model->getTypeFilter())
    {
        if (i == k_EAppTypeGame) {
            ui->appType_game->setChecked(true);
        } 

        if (i == k_EAppTypeApplication) {
            ui->appType_application->setChecked(true);
        }

        if (i == k_EAppTypeMusic) {
            ui->appType_music->setChecked(true);
        }
    }
    
}

FiltersPopup::~FiltersPopup()
{
    delete ui;
}

void FiltersPopup::on_appState_notInstalled_stateChanged(int arg1)
{
    AppState state = model->getStateFilter();
    state.Uninstalled = (bool)arg1;
    model->setStateFilter(state);
}

void FiltersPopup::on_appState_installed_stateChanged(int arg1)
{
    AppState state = model->getStateFilter();
    state.FullyInstalled = (bool)arg1;
    model->setStateFilter(state);
}


void FiltersPopup::on_appState_updateRequired_stateChanged(int arg1)
{
    AppState state = model->getStateFilter();
    state.UpdateRequired = (bool)arg1;
    model->setStateFilter(state);
}


void FiltersPopup::on_appState_updating_stateChanged(int arg1)
{
    AppState state = model->getStateFilter();
    state.UpdateRunning = (bool)arg1;
    model->setStateFilter(state);
}


void FiltersPopup::on_appState_running_stateChanged(int arg1)
{
    AppState state = model->getStateFilter();
    state.AppRunning = (bool)arg1;
    model->setStateFilter(state);
}

void FiltersPopup::updateSelectedTypes() 
{
    std::set<EAppType> newTypes;
    if (ui->appType_application->isChecked()) {
        newTypes.emplace(k_EAppTypeApplication);
    }

    if (ui->appType_game->isChecked()) {
        newTypes.emplace(k_EAppTypeGame);
    }

    if (ui->appType_music->isChecked()) {
        newTypes.emplace(k_EAppTypeMusic);
    }
    
    model->setTypeFilter(newTypes);
}

void FiltersPopup::on_appType_game_stateChanged(int arg1)
{
    updateSelectedTypes();
}

void FiltersPopup::on_appType_application_stateChanged(int arg1)
{
    updateSelectedTypes();
}


void FiltersPopup::on_appType_music_stateChanged(int arg1)
{
    updateSelectedTypes();
}

