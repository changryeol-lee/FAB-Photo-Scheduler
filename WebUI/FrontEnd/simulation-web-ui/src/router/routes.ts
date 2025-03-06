import type { RouteRecordRaw } from 'vue-router'
import MasterData from 'pages/MasterData.vue'
import GanttChart from 'pages/GanttChart.vue'
import GanttChart2 from 'pages/GanttChart2.vue'
import EqpSchedule from 'src/pages/EqpSchedule.vue'
import DispatchLog from 'src/pages/DispatchLog.vue'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('pages/IndexPage.vue') },
      { path: 'master-data', component: MasterData },
      {
        path: 'analysis',
        children: [
          { path: 'gantt', component: GanttChart },
          { path: 'gantt2', component: GanttChart2 },
          { path: 'eqp-schedule', component: EqpSchedule },
          { path: 'dispatch-log', component: DispatchLog },
        ],
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
]

export default routes
