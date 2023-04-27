#include <QVariant>
#include <QList>

#pragma once

enum TreeItemType
{
    k_ETreeItemTypeRoot,
    k_ETreeItemTypeCategory,
    k_ETreeItemTypeApp
};

// An item in a tree
class TreeItem
{
public:
    TreeItem();
    ~TreeItem();
    int row();
    TreeItem *getChildByRow(int row);
    TreeItemType type;
    TreeItem *parent;
    QList<TreeItem *> children;
    
    // Either a category name or a pointer to an app
    QVariant value;
};