#include "qlistwidgetlaunchoptionitem.h"

QListWidgetLaunchOptionItem::QListWidgetLaunchOptionItem(LaunchOption opt)
{
    this->opt = opt;
    this->setText(QString::fromStdString(opt.name));
}

QListWidgetLaunchOptionItem::~QListWidgetLaunchOptionItem()
{
}
