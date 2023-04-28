#include "clickableqwidget.h"
#include "ui_clickableqwidget.h"
#include <QVBoxLayout>
#include <iostream>
#include "../../temporary_logging_solution.h"

ClickableQWidget::ClickableQWidget(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::ClickableQWidget)
{
    ui->setupUi(this);
    this->setCursor(Qt::PointingHandCursor);
}

bool clickState = false;
void ClickableQWidget::mousePressEvent(QMouseEvent *event)
{
    clickState = true;
    event->accept();
}

void ClickableQWidget::mouseReleaseEvent(QMouseEvent *event) 
{
    if (clickState) {
        clickState = false;
        clicked();
    }
    event->accept();
}

void ClickableQWidget::ReplaceExistingQWidget(QWidget *existingWidget) 
{
    this->setLayout(new QVBoxLayout());

    for (auto &&i : existingWidget->findChildren<QWidget*>())
    {
        i->setParent(this);
        this->layout()->addWidget(i);
    }
    
    existingWidget->setVisible(false);
}

ClickableQWidget::~ClickableQWidget()
{
    delete ui;
}
