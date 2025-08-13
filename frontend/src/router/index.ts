import { createRouter, createWebHistory } from 'vue-router'
import MenuLayout from '@/layouts/MenuLayout.vue'
import HomeView from '@/views/HomeView.vue'
import StudentView from '@/views/StudentsView.vue'

const routes = [
  {
    path: '/',
    component: MenuLayout,
    children: [
      {
        path: '',
        name: 'Home',
        component: HomeView,
      },
      {
        path: 'alunos',
        name: 'Alunos',
        component: StudentView,
      },
    ],
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router