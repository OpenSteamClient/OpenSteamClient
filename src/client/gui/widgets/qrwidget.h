#include <QWidget>

#pragma once

class QRWidget : public QWidget {
    Q_OBJECT
private:
    QString data;
public:
    explicit QRWidget(QWidget *parent = 0);
    void setQRData(QString data);

protected:
    void paintEvent(QPaintEvent *);
};
