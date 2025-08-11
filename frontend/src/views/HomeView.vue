<template>
  <div>
    <h1>Lista de Estudantes</h1>
    <ul>
      <li v-for="student in students" :key="student.id">
        {{ student.name }} 
      </li>
    </ul>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { getAllStudents } from '../services/studentService'
import type { Student } from '../types/Student'

const students = ref<Student[]>([])

onMounted(async () => {
  students.value = await getAllStudents()
})
</script>