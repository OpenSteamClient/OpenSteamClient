#include "treeitem.h"
#include <iostream>
#include "../../interop/app.h"

TreeItem::TreeItem(TreeItem *parent, TreeItemType type) {
    this->type = type;
    this->parent = parent;
    if (parent != nullptr) {
        parent->AddChild(this);
    }
    this->text = "";
    this->value = QVariant(QString(""));
}

TreeItem::~TreeItem() {

}

int TreeItem::row() 
{
    if (this->parent)
        return this->parent->childrenFlat.indexOf(const_cast<TreeItem*>(this));

    return 0;
}

void TreeItem::AddChild(TreeItem *item) {
    std::map<std::string, TreeItem *> items;
    std::pair<std::string, TreeItem *> favoriteCategory = {"", nullptr};

    for (auto &&i : this->childrenFlat)
    {
        if (i->text == "FAVORITES") {
            favoriteCategory = {i->text, i};
        } else {
            items.insert({i->text, i});
        }
    }
    
    items.insert({item->text, item});

    this->childrenFlat.clear();

    if (favoriteCategory.second != nullptr) {
        this->childrenFlat.append(favoriteCategory.second);
    }

    for (auto &&i : items)
    {
        this->childrenFlat.append(i.second);
    }
}

TreeItem *TreeItem::getChildByRow(int row)
{
    if (row < 0 || row >= this->childrenFlat.size())
        return nullptr;
    return this->childrenFlat.at(row);
}