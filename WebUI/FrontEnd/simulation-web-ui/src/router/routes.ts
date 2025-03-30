import type { RouteRecordRaw } from 'vue-router'
import MasterData from 'pages/MasterData.vue'
import GanttChart from 'pages/GanttChart.vue'
import EqpSchedule from 'src/pages/Planning/EqpSchedule.vue'
import DispatchLog from 'src/pages/DispatchLog.vue'
import BomData from 'src/pages/Master/BomData.vue'
import EquipmentData from 'src/pages/Master/EquipmentData.vue'
import MasterDataLayout from 'src/layouts/MasterDataLayout.vue'
import EngineExecute from 'pages/Planning/EngineExecute.vue'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', redirect: '/analysis/gantt' },
      {
        path: 'master',
        children: [
          { path: 'bom', component: BomData },
          { path: 'equipment', component: EquipmentData },
          // { path: 'lot', component: LotData },
          // { path: 'etc', component: EtcData },
        ],
      },
      {
        path: 'planning',
        children: [
          { path: 'engine-execute', component: EngineExecute },
          { path: 'eqp-schedule', component: EqpSchedule },
        ],
      },
      {
        path: 'analysis',
        children: [
          { path: 'gantt', component: GanttChart },
          // { path: 'dispatch-log', component: DispatchLog },
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
