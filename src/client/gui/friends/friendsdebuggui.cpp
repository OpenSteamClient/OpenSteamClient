#include "friendsdebuggui.h"
#include "ui_friendsdebuggui.h"
#include "../../ext/steamclient.h"
#include <opensteamworks/IClientFriends.h>

FriendsDebugGui::FriendsDebugGui(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::FriendsDebugGui)
{
    ui->setupUi(this);
}

FriendsDebugGui::~FriendsDebugGui()
{
    delete ui;
}

void FriendsDebugGui::on_pushButton_clicked()
{
    Global_SteamClientMgr->ClientFriends->SetListenForFriendsMessages(true);
    Global_SteamClientMgr->ClientFriends->SetPersonaState(k_EPersonaStateOnline);
}

