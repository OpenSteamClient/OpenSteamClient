#include <QVariant>
#include <QList>
#include <string>

#pragma once

enum TreeItemType
{
    k_ETreeItemTypeRoot,
    k_ETreeItemTypeCategory,
    k_ETreeItemTypeApp,
    k_ETreeItemTypeInvalid = -1
};

// An item in a tree
class TreeItem
{
public:
    TreeItem(TreeItem *parent, TreeItemType type);
    ~TreeItem();
    int row();
    TreeItem *getChildByRow(int row);
    void AddChild(TreeItem *);
    TreeItemType type;
    TreeItem *parent;

    std::string text;

    QList<TreeItem *> childrenFlat;

    // Either a category name or a pointer to an app
    QVariant value;
};