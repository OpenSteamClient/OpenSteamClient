#ifndef FRIENDSDEBUGGUI_H
#define FRIENDSDEBUGGUI_H

#include <QDialog>

namespace Ui {
class FriendsDebugGui;
}

class FriendsDebugGui : public QDialog
{
    Q_OBJECT

public:
    explicit FriendsDebugGui(QWidget *parent = nullptr);
    ~FriendsDebugGui();

private slots:
    void on_pushButton_clicked();

private:
    Ui::FriendsDebugGui *ui;
};

#endif // FRIENDSDEBUGGUI_H
