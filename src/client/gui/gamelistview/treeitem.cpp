#include "treeitem.h"

TreeItem::TreeItem() {

}

TreeItem::~TreeItem() {

}

int TreeItem::row() 
{
    if (this->parent)
        return this->parent->children.indexOf(const_cast<TreeItem*>(this));

    return 0;
}

TreeItem *TreeItem::getChildByRow(int row)
{
    if (row < 0 || row >= this->children.size())
        return nullptr;
    return this->children.at(row);
}