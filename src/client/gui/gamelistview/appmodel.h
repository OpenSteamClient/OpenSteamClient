#include <QAbstractListModel>
#include <QList>
#include <QItemSelection>
#include <vector>

#include "../../interop/app.h"
#include "treeitem.h"



#pragma once


class AppModel : public QAbstractItemModel 
{
  Q_OBJECT
public:
  explicit AppModel(QObject *parent = nullptr);
  int rowCount(const QModelIndex &parent = QModelIndex()) const;
  int columnCount(const QModelIndex &parent = QModelIndex()) const;
  QVariant data(const QModelIndex &index, int role) const;
  QVariant headerData(int section, Qt::Orientation orientation, int role) const;
  Qt::ItemFlags flags(const QModelIndex &index) const;
  QModelIndex index(int row, int column, const QModelIndex &parent = QModelIndex()) const;
  QModelIndex parent(const QModelIndex &index) const;
  bool containsCategory(QString category);
  TreeItem *findCategory(QString category);

  void addApp(std::vector<std::string> categories, App *app);

private:
  std::vector<TreeItem*> tree;
  TreeItem *rootItem;
};

Q_DECLARE_METATYPE(AppModel)