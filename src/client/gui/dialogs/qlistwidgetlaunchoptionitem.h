#include <QListWidgetItem>
#include "../../interop/app.h"

#pragma once

class QListWidgetLaunchOptionItem: public QListWidgetItem
{
private:
public:
    LaunchOption opt;
    QListWidgetLaunchOptionItem(LaunchOption opt);
    QListWidgetLaunchOptionItem(QListWidget *parent) : QListWidgetItem(parent){};
    ~QListWidgetLaunchOptionItem();
};