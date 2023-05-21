#include <QAbstractListModel>
#include <QList>
#include <QItemSelection>
#include <vector>
#include <set>

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

  void addApp(std::vector<std::string> categories, App *app, bool initial = false);

  // Filtering

  // Filter by type
  std::set<EAppType> getTypeFilter();
  void setTypeFilter(std::set<EAppType> types);
  void clearTypeFilter();

  // Filter by name
  std::string getNameContainsFilter();
  void setNameContainsFilter(std::string contains);
  void clearNameContainsFilter();

  // Filter by tags (Multiplayer, etc)
  //TODO: tag filtering

  // Filter by state
  AppState getStateFilter();
  void setStateFilter(AppState state);
  void clearStateFilter();

  // Filters the list with criteria specified by the _Filter functions.
  void filter();
signals:
  void sortingFinishedd();

private slots:
  void appStateChanged();

private:
  std::vector<TreeItem *> tree;
  std::vector<TreeItem *> treeFiltered;
  TreeItem *rootItem;
  TreeItem *rootItemFiltered;

  // Filtering

  std::set<EAppType> typeFilter;
  std::string nameContainsFilter;
  AppState stateFilter;
};

Q_DECLARE_METATYPE(AppModel)