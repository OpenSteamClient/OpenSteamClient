#ifndef CLICKABLEQWIDGET_H
#define CLICKABLEQWIDGET_H

#include <QWidget>
#include <QMouseEvent>

namespace Ui {
class ClickableQWidget;
}

class ClickableQWidget : public QWidget
{
    Q_OBJECT

public:
    explicit ClickableQWidget(QWidget *parent = nullptr);
    ~ClickableQWidget();
    void mousePressEvent(QMouseEvent *event);
    void mouseReleaseEvent(QMouseEvent *event);
    void ReplaceExistingQWidget(QWidget *existingWidget);

Q_SIGNALS:
    void clicked();

private:
    Ui::ClickableQWidget *ui;
};

#endif // CLICKABLEQWIDGET_H
